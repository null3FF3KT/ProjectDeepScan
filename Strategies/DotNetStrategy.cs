using System.IO;
using System.Linq;
using ProjectDeepScan.Services;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Strategies;

public class DotNetStrategy : IProjectStrategy
{
    private readonly CodeAnalyzer? _codeAnalyzer;

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

    public bool IsProjectFile(string fileName)
    {
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
        return string.Format($"Unknown FileType. FileName: {0}", fileName);
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
        return [];
    }
}