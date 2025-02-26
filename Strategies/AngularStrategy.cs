using System.IO;
using System.Text.RegularExpressions;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Strategies;

public class AngularStrategy : IProjectStrategy
{
        // Regex patterns for TypeScript parsing
    private static readonly Regex ClassPattern = new(
        @"export\s+class\s+(\w+)",
        RegexOptions.Compiled);
        
    private static readonly Regex MethodPattern = new(
        @"(?:public|private|protected)?\s*(\w+)\s*\([^)]*\)\s*(?::\s*\w+\s*)?\s*{",
        RegexOptions.Compiled);
        
    private static readonly Regex PropertyPattern = new(
        @"(?:public|private|protected)?\s*(\w+)\s*:\s*\w+(?:<[^>]+>)?(?:\s*=\s*[^;]+)?;",
        RegexOptions.Compiled);
        
    private static readonly Regex DecoratorPattern = new(
        @"@(\w+)\s*\([^)]*\)",
        RegexOptions.Compiled);
        
    private static readonly Regex InputPattern = new(
        @"@Input\(\s*(?:'([^']+)'|""([^""]+)"")?\s*\)\s*(?:public|private|protected)?\s*(\w+)",
        RegexOptions.Compiled);
        
    private static readonly Regex OutputPattern = new(
        @"@Output\(\s*(?:'([^']+)'|""([^""]+)"")?\s*\)\s*(?:public|private|protected)?\s*(\w+)",
        RegexOptions.Compiled);
    public bool CanHandle(string projectPath)
    {
        return File.Exists(Path.Combine(projectPath, "angular.json"));
    }

    public string GetProjectType() => "Angular";

    public bool IsProjectFile(string fileName)
    {
        if (fileName.EndsWith(".ts") && !fileName.EndsWith(".spec.ts"))
        {
            return fileName.Contains(".component.") ||
                   fileName.Contains(".service.") ||
                   fileName.Contains(".module.") ||
                   fileName.Contains(".pipe.") ||
                   fileName.Contains(".directive.") ||
                   fileName.Contains(".guard.") ||
                   fileName.Contains(".resolver.") ||
                   fileName.Contains(".interceptor.");
        }
        
        return fileName == "angular.json" ||
               fileName == "package.json" ||
               fileName == "tsconfig.json" ||
               fileName == "tsconfig.app.json";
    }

    public string GetFileType(string fileName)
    {
        if (fileName.Contains(".component.")) return "Component";
        if (fileName.Contains(".service.")) return "Service";
        if (fileName.Contains(".module.")) return "Module";
        if (fileName.Contains(".pipe.")) return "Pipe";
        if (fileName.Contains(".directive.")) return "Directive";
        if (fileName.Contains(".guard.")) return "Guard";
        if (fileName.Contains(".resolver.")) return "Resolver";
        if (fileName.Contains(".interceptor.")) return "Interceptor";
        if (fileName.EndsWith("angular.json")) return "Configuration";
        if (fileName.EndsWith("package.json")) return "Configuration";
        if (fileName.EndsWith("tsconfig.json")) return "Configuration";
        return "Unknown";
    }

    public string GetComponentName(string fileName)
    {
        var name = Path.GetFileNameWithoutExtension(fileName);
        var parts = name.Split('.');
        return parts[0];
    }

    public List<MemberInfo> GetMembers(string filePath)
    {
        var members = new List<MemberInfo>();
        if (!File.Exists(filePath) || !filePath.EndsWith(".ts") || filePath.EndsWith(".spec.ts")) 
            return members;

        try
        {
            var content = File.ReadAllText(filePath);
            
            // Remove comments to avoid false positives
            content = RemoveComments(content);
            
            // Extract class name
            string? className = null;
            var classMatch = ClassPattern.Match(content);
            if (classMatch.Success)
            {
                className = classMatch.Groups[1].Value;
                members.Add(new MemberInfo
                {
                    Name = className,
                    Type = "Class",
                    Signature = $"export class {className}",
                    AccessLevel = "public"
                });
            }
            
            // Extract decorators
            foreach (Match match in DecoratorPattern.Matches(content))
            {
                string decoratorName = match.Groups[1].Value;
                if (decoratorName is "Component" or "Injectable" or "NgModule" or "Directive" or "Pipe")
                {
                    members.Add(new MemberInfo
                    {
                        Name = decoratorName,
                        Type = "Decorator",
                        Signature = match.Value.Trim(),
                        AccessLevel = "public"
                    });
                }
            }

            // Extract @Input() properties
            foreach (Match match in InputPattern.Matches(content))
            {
                string inputName = match.Groups[3].Value;
                string alias = match.Groups[1].Value.Length > 0 ? match.Groups[1].Value : 
                              (match.Groups[2].Value.Length > 0 ? match.Groups[2].Value : inputName);
                
                members.Add(new MemberInfo
                {
                    Name = inputName,
                    Type = "Input",
                    Signature = $"@Input('{alias}') {inputName}",
                    AccessLevel = "public"
                });
            }
            
            // Extract @Output() events
            foreach (Match match in OutputPattern.Matches(content))
            {
                string outputName = match.Groups[3].Value;
                string alias = match.Groups[1].Value.Length > 0 ? match.Groups[1].Value : 
                              (match.Groups[2].Value.Length > 0 ? match.Groups[2].Value : outputName);
                
                members.Add(new MemberInfo
                {
                    Name = outputName,
                    Type = "Output",
                    Signature = $"@Output('{alias}') {outputName}",
                    AccessLevel = "public"
                });
            }
            
            // Extract methods
            foreach (Match match in MethodPattern.Matches(content))
            {
                string methodName = match.Groups[1].Value;
                // Skip constructor and lifecycle hooks as they're implicit in Angular
                if (methodName != "constructor" && !IsAngularLifecycleHook(methodName))
                {
                    members.Add(new MemberInfo
                    {
                        Name = methodName,
                        Type = "Method",
                        Signature = match.Value.Trim().Replace("{", ""),
                        AccessLevel = DetermineAccessLevel(match.Value)
                    });
                }
            }
            
            // Extract properties
            foreach (Match match in PropertyPattern.Matches(content))
            {
                string propertyName = match.Groups[1].Value;
                // Skip internal Angular properties
                if (!propertyName.StartsWith("_") && 
                    !IsCommonAngularProperty(propertyName) &&
                    !IsOutputEventEmitter(match.Value))
                {
                    members.Add(new MemberInfo
                    {
                        Name = propertyName,
                        Type = "Property",
                        Signature = match.Value.Trim(),
                        AccessLevel = DetermineAccessLevel(match.Value)
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error analyzing TypeScript file {filePath}: {ex.Message}");
        }
        
        return members;
    }
    
    private bool IsOutputEventEmitter(string propertyDeclaration)
    {
        // Check if this is an EventEmitter property (usually used with @Output)
        return propertyDeclaration.Contains("EventEmitter<") || 
               propertyDeclaration.Contains("new EventEmitter");
    }
    
    private bool IsAngularLifecycleHook(string methodName)
    {
        return methodName == "ngOnInit" ||
               methodName == "ngOnChanges" ||
               methodName == "ngDoCheck" ||
               methodName == "ngAfterContentInit" ||
               methodName == "ngAfterContentChecked" ||
               methodName == "ngAfterViewInit" ||
               methodName == "ngAfterViewChecked" ||
               methodName == "ngOnDestroy";
    }
    
    private bool IsCommonAngularProperty(string propertyName)
    {
        return propertyName == "constructor" ||
               propertyName == "ngOnInit" ||
               propertyName == "form";
    }
    
    private string DetermineAccessLevel(string declaration)
    {
        if (declaration.Contains("private "))
            return "private";
        if (declaration.Contains("protected "))
            return "protected";
        return "public"; // Default in TypeScript if not specified
    }
    
    private string RemoveComments(string content)
    {
        // Remove single line comments
        content = Regex.Replace(content, @"\/\/.*$", "", RegexOptions.Multiline);
        
        // Remove multi-line comments
        content = Regex.Replace(content, @"\/\*[\s\S]*?\*\/", "", RegexOptions.Multiline);
        
        return content;
    }
}