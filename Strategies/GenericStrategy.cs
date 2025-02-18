using System.Collections.Generic;
using System.IO;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Strategies;

public class GenericStrategy : IProjectStrategy
{
    private readonly Dictionary<string, string> _fileTypes = new()
    {
        {".js", "JavaScript"},
        {".ts", "TypeScript"},
        {".py", "Python"},
        {".java", "Java"},
        {".rb", "Ruby"},
        {".php", "PHP"},
        {".go", "Go"},
        {".rs", "Rust"},
        {".html", "HTML"},
        {".css", "CSS"},
        {".scss", "SCSS"},
        {".json", "JSON"},
        {".xml", "XML"},
        {".yaml", "YAML"},
        {".yml", "YAML"},
        {".md", "Markdown"},
        {".sql", "SQL"}
    };

    public bool CanHandle(string projectPath) => true; // Fallback strategy

    public string GetProjectType() => "Generic";

    public bool IsProjectFile(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLower();
        return _fileTypes.ContainsKey(ext);
    }

    public string GetFileType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLower();
        return _fileTypes.GetValueOrDefault(ext, "Unknown");
    }

    public string GetComponentName(string fileName)
    {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    public List<MemberInfo> GetMembers(string filePath)
    {
        // Generic strategy doesn't analyze members
        return new List<MemberInfo>();
    }
}