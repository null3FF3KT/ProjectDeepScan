using ProjectDeepScan.Models;
using ProjectDeepScan.Strategies;
using ProjectDeepScan.Services;

namespace ProjectDeepScan.Services;

public class ProjectScannerService
{
    private readonly string _projectPath;
    private readonly IProjectStrategy _strategy;
    private readonly GitIgnoreParser _gitIgnoreParser;
    private readonly ScannerConfig _config;

    public ProjectScannerService(string projectPath, ScannerConfig? config = null)
    {
        _projectPath = projectPath;
        _config = config ?? new ScannerConfig();
        var codeAnalyzer = new CodeAnalyzer();
        _strategy = DetermineProjectType(projectPath, codeAnalyzer);
        _gitIgnoreParser = new GitIgnoreParser(projectPath);
        
        Console.WriteLine($"Detected project type: {_strategy.GetProjectType()}");
        if (_config.AnalyzeMembers)
        {
            Console.WriteLine("Deep Scan enabled - analyzing public members");
        }
    }

    private IProjectStrategy DetermineProjectType(string projectPath, CodeAnalyzer codeAnalyzer)
    {
        var strategies = new List<IProjectStrategy>
        {
            new AngularStrategy(),
            new DotNetStrategy(codeAnalyzer),
            new GenericStrategy()  // Fallback
        };

        return strategies.First(s => s.CanHandle(projectPath));
    }

    public ProjectNode ScanProject()
    {
        var root = new ProjectNode
        {
            Name = Path.GetFileName(_projectPath.TrimEnd(Path.DirectorySeparatorChar)),
            Type = _strategy.GetProjectType(),
            Path = _projectPath
        };

        ScanDirectory(new DirectoryInfo(_projectPath), root);
        return root;
    }

    private void ScanDirectory(DirectoryInfo dir, ProjectNode parent)
    {
        if (dir.FullName != _projectPath && 
            _gitIgnoreParser.ShouldIgnorePath(dir.FullName, _strategy is AngularStrategy))
            return;

        try
        {
            foreach (var file in dir.GetFiles())
            {
                if (_gitIgnoreParser.ShouldIgnorePath(file.FullName, _strategy is AngularStrategy))
                    continue;

                if (_strategy.IsProjectFile(file.Name))
                {
                    var node = new ProjectNode
                    {
                        Name = _strategy.GetComponentName(file.Name),
                        Type = _strategy.GetFileType(file.FullName),
                        Path = file.FullName,
                        Members = _config.AnalyzeMembers ? _strategy.GetMembers(file.FullName) : new List<MemberInfo>()
                    };
                    parent.Children.Add(node);
                }
            }

            foreach (var subDir in dir.GetDirectories())
            {
                if (!_gitIgnoreParser.ShouldIgnorePath(subDir.FullName, _strategy is AngularStrategy))
                {
                    var dirNode = new ProjectNode
                    {
                        Name = subDir.Name,
                        Type = "Directory",
                        Path = subDir.FullName
                    };
                    parent.Children.Add(dirNode);
                    ScanDirectory(subDir, dirNode);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scanning directory {dir.FullName}: {ex.Message}");
        }
    }

    public void PrintTree(ProjectNode node, string indent = "")
    {
        Console.WriteLine($"{indent}├── {node.Name} ({node.Type})");
        if (_config.AnalyzeMembers && node.Members.Count != 0)
        {
            foreach (var member in node.Members)
            {
                Console.WriteLine($"{indent}│   ├── {member.Name}: {member.Type} - {member.Signature}");
            }
        }
        foreach (var child in node.Children)
        {
            PrintTree(child, indent + "│   ");
        }
    }
}