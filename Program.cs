using ProjectDeepScan.Services;
using ProjectDeepScan.Exporters;
using ProjectDeepScan.Models;

namespace ProjectDeepScan;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide the path to your project.");
            Console.WriteLine("Usage: ProjectDeepScan <path> [--deep]");
            Console.WriteLine("  --deep: Include analysis of public members");
            return;
        }

        var config = new ScannerConfig
        {
            AnalyzeMembers = args.Contains("--deep")
        };

        var projectPath = args[0];
        if (projectPath == "--deep")
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide the project path when using --deep");
                return;
            }
            projectPath = args[1];
        }

        var scanner = new ProjectScannerService(projectPath, config);
        var projectStructure = scanner.ScanProject();

        Console.WriteLine("\nProject Structure:");
        scanner.PrintTree(projectStructure);

        var jsonExporter = new JsonProjectExporter();
        jsonExporter.Export(projectStructure, "project-structure.json");
        Console.WriteLine("\nProject structure has been exported to 'project-structure.json'");

        var markdownExporter = new MarkdownProjectExporter();
        markdownExporter.Export(projectStructure, "project-structure.md");
        Console.WriteLine("Project structure has been exported to 'project-structure.md'");
    }
}