using System.IO;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Strategies;

public class AngularStrategy : IProjectStrategy
{
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
        // For now, return empty list for Angular files
        // Could be enhanced later to parse TypeScript files
        return new List<MemberInfo>();
    }
}