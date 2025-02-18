using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectDeepScan.Services;

public class GitIgnoreParser
{
    private readonly string _projectPath;
    private readonly List<string> _gitIgnorePatterns;
    private readonly HashSet<string> _baseIgnoreDirs;

    public GitIgnoreParser(string projectPath)
    {
        _projectPath = projectPath;
        _baseIgnoreDirs = new HashSet<string>
        {
            "node_modules",
            "dist",
            "tmp",
            "out-tsc",
            "bazel-out",
            "coverage",
            "bin",
            "obj",
            "Debug",
            "Release"
        };
        _gitIgnorePatterns = LoadGitIgnore(projectPath);
    }

    private List<string> LoadGitIgnore(string projectPath)
    {
        var patterns = new List<string>();
        var gitIgnorePath = Path.Combine(projectPath, ".gitignore");
        
        if (File.Exists(gitIgnorePath))
        {
            patterns = File.ReadAllLines(gitIgnorePath)
                .Where(line => 
                    !string.IsNullOrWhiteSpace(line) && 
                    !line.StartsWith("#") &&
                    !line.StartsWith("!"))
                .Select(line => 
                {
                    line = line.Trim();
                    if (line.StartsWith("/"))
                        line = line.Substring(1);
                    return line;
                })
                .ToList();
            
            Console.WriteLine($"Loaded {patterns.Count} patterns from .gitignore");
        }
        else
        {
            Console.WriteLine("No .gitignore file found, using default ignore patterns.");
        }

        return patterns;
    }

    private string GlobToRegex(string glob)
    {
        if (string.IsNullOrWhiteSpace(glob))
            return "$^";

        string regex = "";
        
        if (glob.StartsWith("**"))
            regex = ".*";
        else
            regex = "^";
            
        regex += Regex.Escape(glob)
            .Replace("\\*\\*/", "(.*/)?")
            .Replace("\\*\\*", ".*")
            .Replace("\\*", "[^/]*")
            .Replace("\\?", "[^/]")
            .Replace("\\[", "[")
            .Replace("\\]", "]");

        if (glob.EndsWith("/"))
            regex += ".*";
        
        return regex + "$";
    }

    public bool ShouldIgnorePath(string path, bool isAngularProject = false)
    {
        var relativePath = Path.GetRelativePath(_projectPath, path).Replace('\\', '/');
        var pathSegments = relativePath.Split(Path.DirectorySeparatorChar);

        // Don't treat root directory "." as hidden
        if (relativePath != "." && pathSegments.Any(segment => 
            segment.StartsWith(".") && 
            !segment.Equals(".gitignore") && 
            !segment.StartsWith(".package")))
        {
            return true;
        }

        if (_baseIgnoreDirs.Contains(pathSegments[0]))
        {
            return true;
        }

        // Never ignore the src directory or its immediate children for Angular projects
        if (isAngularProject && (relativePath.StartsWith("src") || relativePath == "src"))
        {
            return false;
        }

        foreach (var pattern in _gitIgnorePatterns)
        {
            var regexPattern = GlobToRegex(pattern);
            try 
            {
                if (Regex.IsMatch(relativePath, regexPattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error matching pattern {pattern}: {ex.Message}");
            }
        }

        return false;
    }
}