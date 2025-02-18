# . Project Structure
*Project Type: .NET*

Generated on: 2025-02-18 10:33:22

## Directory Structure
```
└── . (.NET)
    ├── ProjectDeepScan (Solution)
    ├── ProjectDeepScan (Project)
    ├── Program (Class)
    │   ├── (2 members)
    ├── Exporters (Directory)
    │   ├── IProjectExporter (Interface)
    │   │   ├── (1 members)
    │   ├── MarkdownProjectExporter (Class)
    │   │   ├── (1 members)
    │   └── JsonProjectExporter (Class)
    │       ├── (1 members)
    ├── Services (Directory)
    │   ├── ProjectScannerService (Class)
    │   │   ├── (4 members)
    │   ├── GitIgnoreParser (Class)
    │   │   ├── (2 members)
    │   └── CodeAnalyzer (Class)
    │       ├── (3 members)
    ├── Strategies (Directory)
    │   ├── GenericStrategy (Class)
    │   │   ├── (6 members)
    │   ├── IProjectStrategy (Interface)
    │   │   ├── (1 members)
    │   ├── AngularStrategy (Class)
    │   │   ├── (6 members)
    │   └── DotNetStrategy (Class)
    │       ├── (6 members)
    └── Models (Directory)
        ├── MemberInfo (Class)
        │   ├── (4 members)
        ├── ProjectNode (Class)
        │   ├── (5 members)
        └── ScannerConfig (Class)
            ├── (1 members)
```

## Detailed Type Information

### Program/Program
**Type**: Class

**Members:**

#### Methods
- `public static void Main(string[] args)`

#### Properties
- `public class Program`

### Exporters/IProjectExporter/IProjectExporter
**Type**: Interface

**Members:**

#### Properties
- `public interface IProjectExporter`

### Exporters/MarkdownProjectExporter/MarkdownProjectExporter
**Type**: Class

**Members:**

#### Methods
- `public void Export(ProjectNode node, string outputPath)`

### Exporters/JsonProjectExporter/JsonProjectExporter
**Type**: Class

**Members:**

#### Methods
- `public void Export(ProjectNode node, string outputPath)`

### Services/ProjectScannerService/ProjectScannerService
**Type**: Class

**Members:**

#### Methods
- `public void PrintTree(ProjectNode node, string indent = "")`
- `public ProjectNode ScanProject()`

#### Properties
- `public class ProjectScannerService`
- `public ProjectNode ScanProject()`

### Services/GitIgnoreParser/GitIgnoreParser
**Type**: Class

**Members:**

#### Methods
- `public bool ShouldIgnorePath(string path, bool isAngularProject = false)`

#### Properties
- `public class GitIgnoreParser`

### Services/CodeAnalyzer/CodeAnalyzer
**Type**: Class

**Members:**

#### Events
- `public event {match.Groups[1].Value} {match.Groups[2].Value}",`

#### Methods
- `public List<MemberInfo> AnalyzeFile(string filePath)`

#### Properties
- `public class CodeAnalyzer`

### Strategies/GenericStrategy/GenericStrategy
**Type**: Class

**Members:**

#### Methods
- `public bool CanHandle(string projectPath)`
- `public string GetComponentName(string fileName)`
- `public string GetFileType(string fileName)`
- `public List<MemberInfo> GetMembers(string filePath)`
- `public string GetProjectType()`
- `public bool IsProjectFile(string fileName)`

### Strategies/IProjectStrategy/IProjectStrategy
**Type**: Interface

**Members:**

#### Properties
- `public interface IProjectStrategy`

### Strategies/AngularStrategy/AngularStrategy
**Type**: Class

**Members:**

#### Methods
- `public bool CanHandle(string projectPath)`
- `public string GetComponentName(string fileName)`
- `public string GetFileType(string fileName)`
- `public List<MemberInfo> GetMembers(string filePath)`
- `public string GetProjectType()`
- `public bool IsProjectFile(string fileName)`

### Strategies/DotNetStrategy/DotNetStrategy
**Type**: Class

**Members:**

#### Methods
- `public bool CanHandle(string projectPath)`
- `public string GetComponentName(string fileName)`
- `public string GetFileType(string fileName)`
- `public List<MemberInfo> GetMembers(string filePath)`
- `public string GetProjectType()`
- `public bool IsProjectFile(string fileName)`

### Models/MemberInfo/MemberInfo
**Type**: Class

**Members:**

#### Properties
- `public string AccessLevel`
- `public class MemberInfo`
- `public string Signature`
- `public string Type`

### Models/ProjectNode/ProjectNode
**Type**: Class

**Members:**

#### Properties
- `public List<ProjectNode> Children`
- `public List<MemberInfo> Members`
- `public string Path`
- `public class ProjectNode`
- `public string Type`

### Models/ScannerConfig/ScannerConfig
**Type**: Class

**Members:**

#### Properties
- `public class ScannerConfig`

## File Type Summary
- **Class**: 12 files
- **Interface**: 2 files
- **.NET**: 1 files
- **Solution**: 1 files
