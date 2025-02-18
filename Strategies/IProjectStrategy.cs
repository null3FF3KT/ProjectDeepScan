using ProjectDeepScan.Models;

namespace ProjectDeepScan.Strategies;

public interface IProjectStrategy
{
    bool CanHandle(string projectPath);
    string GetProjectType();
    bool IsProjectFile(string fileName);
    string GetFileType(string fileName);
    string GetComponentName(string fileName);

    List<MemberInfo> GetMembers(string filePath);
}