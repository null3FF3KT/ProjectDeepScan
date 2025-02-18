# . Project Structure
*Project Type: .NET*

Generated on: 2025-02-17 20:42:40

## Directory Structure
```
└── . (.NET)
    ├── ProjectDeepScan (Solution)
    ├── ProjectDeepScan (Project)
    ├── Program (Class)
    ├── Exporters (Directory)
    │   ├── IProjectExporter (Interface)
    │   ├── MarkdownProjectExporter (Class)
    │   └── JsonProjectExporter (Class)
    ├── Services (Directory)
    │   ├── ProjectScannerService (Class)
    │   ├── GitIgnoreParser (Class)
    │   └── CodeAnalyzer (Class)
    ├── Strategies (Directory)
    │   ├── GenericStrategy (Class)
    │   ├── IProjectStrategy (Interface)
    │   ├── AngularStrategy (Class)
    │   └── DotNetStrategy (Class)
    └── Models (Directory)
        ├── MemberInfo (Class)
        └── ProjectNode (Class)
```

## Detailed Type Information

### Program/Program
**Type**: Class

**Members:**

#### Methods
- `public static void Main(string[] args)`

#### Propertys
- `public class Program`

### Exporters/IProjectExporter/IProjectExporter
**Type**: Interface

**Members:**

#### Propertys
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

#### Propertys
- `public class ProjectScannerService`
- `public ProjectNode ScanProject()`

### Services/GitIgnoreParser/GitIgnoreParser
**Type**: Class

**Members:**

#### Methods
- `public bool ShouldIgnorePath(string path, bool isAngularProject = false)`

#### Propertys
- `public class GitIgnoreParser`

### Services/CodeAnalyzer/CodeAnalyzer
**Type**: Class

**Members:**

#### Events
- `public event {match.Groups[1].Value} {match.Groups[2].Value}",`

#### Methods
- `public List<MemberInfo> AnalyzeFile(string filePath)`

#### Propertys
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

#### Propertys
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

#### Propertys
- `public DotNetStrategy(CodeAnalyzer codeAnalyzer)`

### Models/MemberInfo/MemberInfo
**Type**: Class

**Members:**

#### Propertys
- `public string AccessLevel`
- `public class MemberInfo`
- `public string Signature`
- `public string Type`

### Models/ProjectNode/ProjectNode
**Type**: Class

**Members:**

#### Propertys
- `public List<ProjectNode> Children`
- `public List<MemberInfo> Members`
- `public string Path`
- `public class ProjectNode`
- `public string Type`
## File Type Summary
- **Class**: 11 files
- **Interface**: 2 files
- **.NET**: 1 files
- **Solution**: 1 files
