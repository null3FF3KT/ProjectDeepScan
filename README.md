# ProjectDeepScan

A powerful .NET tool for scanning and analyzing project structures, providing detailed documentation and insights into your codebase.

## Features

- **Multi-Project Support**: Automatically detects and properly handles different project types (.NET, Angular, and generic fallback for others)
- **Intelligent File Analysis**: Identifies components, services, modules, and other file types based on project structure
- **Member Analysis**: Extracts public methods, properties, and events from source files
- **Smart Filtering**: Respects .gitignore patterns and intelligently skips build artifacts and generated files
- **Flexible Export Options**: Export project structure as JSON or Markdown documentation
- **Visual Project Tree**: Displays an intuitive tree view of your project in the console
- **Deep Scan Mode**: Optional detailed analysis of public members across your codebase

## Installation

### Prerequisites

- .NET 8.0 SDK or later

### Building from Source

```bash
git clone https://github.com/yourusername/ProjectDeepScan.git
cd ProjectDeepScan
dotnet build
```

## Usage

```bash
# Basic scan (project structure only)
dotnet run -- /path/to/your/project

# Deep scan (includes analysis of public members)
dotnet run -- /path/to/your/project --deep

# Alternative deep scan syntax
dotnet run -- --deep /path/to/your/project
```

### Output

ProjectDeepScan generates two files in the project directory:

1. `project-structure.json` - Structured data representation of your project
2. `project-structure.md` - Human-readable Markdown documentation with:
   - Directory structure visualization
   - Detailed type information
   - File type summary statistics

## Project Structure

The tool uses a strategy pattern to support different project types:

- `DotNetStrategy`: Handles .NET projects with C# files, csproj, sln
- `AngularStrategy`: Recognizes Angular components, services, modules, etc.
- `GenericStrategy`: Fallback for other project types

## Architecture

The application follows clean architecture principles:

- **Models**: Core data structures (`ProjectNode`, `MemberInfo`, `ScannerConfig`)
- **Strategies**: Project type detection and file analysis logic
- **Services**: Core scanning and analysis functionality
- **Exporters**: Output generation for different formats

## Example Output

### Directory Tree

```
└── MyProject (.NET)
    ├── MyProject (Solution)
    ├── MyProject.Core (Project)
    │   ├── (5 members)
    ├── Controllers (Directory)
    │   ├── UserController (Class)
    │   │   ├── (3 members)
    │   └── ProductController (Class)
    │       ├── (4 members)
    └── Models (Directory)
        ├── User (Class)
        │   ├── (2 members)
        └── Product (Class)
            ├── (3 members)
```

### Detailed Member Information

```markdown
### Controllers/UserController
**Type**: Class

**Members:**

#### Methods
- `public async Task<IActionResult> GetUsers()`
- `public async Task<IActionResult> GetUser(int id)`
- `public async Task<IActionResult> CreateUser(UserDto user)`
```

## Extensibility

To add support for a new project type:

1. Create a new strategy class implementing `IProjectStrategy`
2. Add your strategy to the list in `ProjectScannerService.DetermineProjectType()`

## License

[MIT License](LICENSE)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
