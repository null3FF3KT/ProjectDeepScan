using System.Text;
using ProjectDeepScan.Models;

namespace ProjectDeepScan.Exporters;

public class MarkdownProjectExporter : IProjectExporter
{
    public void Export(ProjectNode node, string outputPath)
    {
        var fullPath = Path.Combine(node.Path, outputPath);
        var content = new StringBuilder();
        
        // Add header
        content.AppendLine($"# {node.Name} Project Structure");
        content.AppendLine($"*Project Type: {node.Type}*");
        content.AppendLine();
        
        // Add timestamp
        content.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        content.AppendLine();
        
        // Add structure
        content.AppendLine("## Directory Structure");
        content.AppendLine("```");
        BuildMarkdownTree(node, "", true, content);
        content.AppendLine("```");
        content.AppendLine();
        
        // Add detailed member information
        content.AppendLine("## Detailed Type Information");
        BuildDetailedInformation(node, content);

        // Add file type summary
        content.AppendLine("## File Type Summary");
        var fileTypes = GetFileTypeSummary(node);
        foreach (var (type, count) in fileTypes)
        {
            content.AppendLine($"- **{type}**: {count} files");
        }
        
        File.WriteAllText(fullPath, content.ToString());
    }

    private void BuildMarkdownTree(ProjectNode node, string prefix, bool isLast, StringBuilder content)
    {
        var marker = isLast ? "└── " : "├── ";
        content.AppendLine($"{prefix}{marker}{node.Name} ({node.Type})");
        
        for (int i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            var newPrefix = prefix + (isLast ? "    " : "│   ");
            BuildMarkdownTree(child, newPrefix, i == node.Children.Count - 1, content);
        }
    }

    private void BuildDetailedInformation(ProjectNode node, StringBuilder content, string prefix = "")
    {
        if (node.Members.Any())
        {
            content.AppendLine($"\n### {prefix}{node.Name}");
            content.AppendLine($"**Type**: {node.Type}");
            content.AppendLine("\n**Members:**");
            
            // Group members by type
            var groupedMembers = node.Members
                .GroupBy(m => m.Type)
                .OrderBy(g => g.Key);

            foreach (var group in groupedMembers)
            {
                content.AppendLine($"\n#### {group.Key}s");
                foreach (var member in group.OrderBy(m => m.Name))
                {
                    content.AppendLine($"- `{member.Signature}`");
                }
            }
        }

        foreach (var child in node.Children)
        {
            BuildDetailedInformation(child, content, $"{prefix}{child.Name}/");
        }
    }

    private Dictionary<string, int> GetFileTypeSummary(ProjectNode node)
    {
        var summary = new Dictionary<string, int>();
        
        void CountFiles(ProjectNode current)
        {
            if (current.Type != "Directory" && current.Type != "Project")
            {
                if (!summary.ContainsKey(current.Type))
                    summary[current.Type] = 0;
                summary[current.Type]++;
            }
            
            foreach (var child in current.Children)
            {
                CountFiles(child);
            }
        }
        
        CountFiles(node);
        return summary.OrderByDescending(x => x.Value)
                     .ThenBy(x => x.Key)
                     .ToDictionary(x => x.Key, x => x.Value);
    }
}