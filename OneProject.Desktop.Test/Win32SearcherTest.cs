namespace OneProject.Desktop.Test;

using OneProject.Desktop.Infrastructures;
using Serilog;

public class Win32SearcherTest : TestBase<Win32SearcherTest>
{
    [Fact]
    public void search_all_and_print_to_log()
    {
        Log.Information(JsonSerializer.Serialize(Win32Searcher.GetDiskDrive(), JsonSerializerHelper.ToReadonlyJson));
        Log.Information(new string('*',100));
        Log.Information(JsonSerializer.Serialize(Win32Searcher.GetLogicalDisk(), JsonSerializerHelper.ToReadonlyJson));
        Log.Information(new string('*',100));
        Log.Information(JsonSerializer.Serialize(Win32Searcher.GetPhysicalMemory(), JsonSerializerHelper.ToReadonlyJson));
        Log.Information(new string('*',100));
        Log.Information(JsonSerializer.Serialize(Win32Searcher.GetProcessor(), JsonSerializerHelper.ToReadonlyJson));
        Log.Information(new string('*',100));
        Log.Information(JsonSerializer.Serialize(Win32Searcher.GetBaseBoard(), JsonSerializerHelper.ToReadonlyJson));
        Log.Information(new string('*',100));
        Log.Information(JsonSerializer.Serialize(Win32Searcher.GetVideoController(), JsonSerializerHelper.ToReadonlyJson));
    }
}
