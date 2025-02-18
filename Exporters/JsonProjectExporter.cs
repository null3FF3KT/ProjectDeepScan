using System.Text.Json;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Exporters;

public class JsonProjectExporter : IProjectExporter
{
    public void Export(ProjectNode node, string outputPath)
    {
        var fullPath = Path.Combine(node.Path, outputPath);
        var options = new JsonSerializerOptions { 
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(node, options);
        File.WriteAllText(fullPath, json);
    }
}