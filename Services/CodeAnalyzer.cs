using System.Text.RegularExpressions;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Services;

public class CodeAnalyzer
{
    // Regex patterns for different member types
    private static readonly Regex MethodPattern = new(
        @"public\s+(async\s+)?(static\s+)?[^\s]+\s+([^\s\(]+)\s*\([^\)]*\)",
        RegexOptions.Compiled);

    private static readonly Regex PropertyPattern = new(
        @"public\s+([^\s]+)\s+([^\s\{]+)\s*\{[^\}]*\}",
        RegexOptions.Compiled);

    private static readonly Regex EventPattern = new(
        @"public\s+event\s+([^\s]+)\s+([^\s\;]+)",
        RegexOptions.Compiled);

    public List<MemberInfo> AnalyzeFile(string filePath)
    {
        var members = new List<MemberInfo>();
        if (!File.Exists(filePath)) return members;

        var content = File.ReadAllText(filePath);
        
        // Remove comments to avoid false positives
        content = RemoveComments(content);

        // Find methods
        foreach (Match match in MethodPattern.Matches(content))
        {
            members.Add(new MemberInfo
            {
                Name = match.Groups[3].Value,
                Type = "Method",
                Signature = match.Value.Trim(),
                AccessLevel = "public"
            });
        }

        // Find properties
        foreach (Match match in PropertyPattern.Matches(content))
        {
            members.Add(new MemberInfo
            {
                Name = match.Groups[2].Value,
                Type = "Property",
                Signature = $"public {match.Groups[1].Value} {match.Groups[2].Value}",
                AccessLevel = "public"
            });
        }

        // Find events
        foreach (Match match in EventPattern.Matches(content))
        {
            members.Add(new MemberInfo
            {
                Name = match.Groups[2].Value,
                Type = "Event",
                Signature = $"public event {match.Groups[1].Value} {match.Groups[2].Value}",
                AccessLevel = "public"
            });
        }

        return members;
    }

    private string RemoveComments(string content)
    {
        // Remove single-line comments
        content = Regex.Replace(content, @"//.*$", "", RegexOptions.Multiline);
        
        // Remove multi-line comments
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);
        
        return content;
    }
}