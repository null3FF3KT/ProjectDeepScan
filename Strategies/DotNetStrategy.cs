using System.IO;
using System.Linq;
using System.Xml.Linq;
using ProjectDeepScan.Services;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Strategies;

public class DotNetStrategy : IProjectStrategy
{
    private readonly CodeAnalyzer? _codeAnalyzer;
    private readonly HashSet<string> _testFrameworks = new()
    {
        "Microsoft.NET.Test.Sdk",
        "NUnit",
        "xunit",
        "MSTest",
        "nunit.framework",
        "xunit.runner",
        "MSTest.TestFramework"
    };

    public DotNetStrategy(CodeAnalyzer? codeAnalyzer = null)
    {
        _codeAnalyzer = codeAnalyzer;
    }

    public bool CanHandle(string projectPath)
    {
        return Directory.GetFiles(projectPath, "*.csproj").Any() ||
               Directory.GetFiles(projectPath, "*.sln").Any();
    }

    public string GetProjectType() => ".NET";

    private bool IsTestProject(string path)
    {
        // Check if the path contains 'test' or 'tests' directory
        var normalizedPath = path.Replace('\\', '/').ToLowerInvariant();
        if (normalizedPath.Contains("/test/") || normalizedPath.Contains("/tests/"))
            return true;

        // Check if the filename contains Test
        var fileName = Path.GetFileNameWithoutExtension(path);
        if (fileName.EndsWith("Test") || fileName.EndsWith("Tests"))
            return true;

        // If it's a project file, check for test framework references
        if (path.EndsWith(".csproj") && File.Exists(path))
        {
            try
            {
                var projectXml = XDocument.Load(path);
                var packageReferences = projectXml.Descendants("PackageReference")
                    .Select(x => x.Attribute("Include")?.Value)
                    .Where(x => x != null);

                return packageReferences.Any(package => 
                    _testFrameworks.Any(framework => 
                        package!.StartsWith(framework, StringComparison.OrdinalIgnoreCase)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading project file {path}: {ex.Message}");
            }
        }

        return false;
    }

    public bool IsProjectFile(string fileName)
    {
        if (IsTestProject(fileName))
            return false;

        return fileName.EndsWith(".cs") ||
               fileName.EndsWith(".csproj") ||
               fileName.EndsWith(".sln") ||
               fileName.EndsWith(".razor") ||
               fileName.EndsWith(".cshtml");
    }

    public string GetFileType(string fileName)
    {
        if (fileName.EndsWith(".cs"))
        {
            // Try to detect type from file content
            var content = File.ReadAllText(fileName);
            if (content.Contains(" class ")) return "Class";
            if (content.Contains(" interface ")) return "Interface";
            if (content.Contains(" enum ")) return "Enum";
            if (content.Contains(" record ")) return "Record";
            return "Code";
        }
        if (fileName.EndsWith(".csproj")) return "Project";
        if (fileName.EndsWith(".sln")) return "Solution";
        if (fileName.EndsWith(".razor")) return "Razor Component";
        if (fileName.EndsWith(".cshtml")) return "Razor View";
        return "Unknown";
    }

    public string GetComponentName(string fileName)
    {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    public List<MemberInfo> GetMembers(string filePath)
    {
        if (_codeAnalyzer != null && filePath.EndsWith(".cs"))
        {
            return _codeAnalyzer.AnalyzeFile(filePath);
        }
        return new List<MemberInfo>();
    }
}