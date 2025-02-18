namespace ProjectDeepScan.Models;

public class MemberInfo
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";  // Method, Property, Event, etc.
    public string Signature { get; set; } = "";  // Full signature including return type, parameters
    public string AccessLevel { get; set; } = ""; // public, protected, etc.
}