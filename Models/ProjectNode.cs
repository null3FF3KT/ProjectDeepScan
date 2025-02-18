namespace ProjectDeepScan.Models;

public class ProjectNode
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Path { get; set; } = "";
    public List<ProjectNode> Children { get; set; } = new List<ProjectNode>();
    public List<MemberInfo> Members { get; set; } = new List<MemberInfo>(); 
}