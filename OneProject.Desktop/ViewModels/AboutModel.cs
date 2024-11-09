namespace OneProject.Desktop.ViewModels;

using System.Runtime.InteropServices;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;

public class AboutModel : ModelBase<About>
{
    public string MachineName { get; } = Environment.MachineName;
    public string UserName { get; } = Environment.UserName;
    public int ProcessId { get; } = Environment.ProcessId;
    public string? ProcessPath { get; } = Environment.ProcessPath;
    public string RuntimeInfo { get; } = $"{RuntimeInformation.FrameworkDescription} {RuntimeInformation.RuntimeIdentifier}";
}
