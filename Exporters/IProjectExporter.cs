using ProjectDeepScan.Models;

namespace ProjectDeepScan.Exporters;

public interface IProjectExporter
{
    void Export(ProjectNode node, string outputPath);
}