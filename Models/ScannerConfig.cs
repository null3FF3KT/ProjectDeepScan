namespace ProjectDeepScan.Models;

public class ScannerConfig
{
    public bool AnalyzeMembers { get; set; } = false;  // Default to basic scan
    
    // We could add more config options here in the future
    // public bool AnalyzePrivateMembers { get; set; } = false;
    // public bool IncludeDocumentation { get; set; } = false;
    // etc.
}