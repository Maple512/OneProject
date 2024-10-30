namespace OneProject.Desktop.ViewModels;

using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;

public class WindowsInfoModel : ModelBase<WindowsInfo>
{
    public WindowsInfoModel()
    {
        CPU = Win32Searcher.GetProcessor().ToList();
        GPU = Win32Searcher.GetVideoController().ToList();
        DiskDrive = Win32Searcher.GetDiskDrive().ToList();
        LogicalDisk = Win32Searcher.GetLogicalDisk().ToList();
        BaseBoard = Win32Searcher.GetBaseBoard().ToList();
        Memory = Win32Searcher.GetPhysicalMemory().ToList();
    }

    public IEnumerable<IEnumerable<Win32PropertyModel>> GPU { get; }
    public IEnumerable<IEnumerable<Win32PropertyModel>> DiskDrive { get; }
    public IEnumerable<IEnumerable<Win32PropertyModel>> LogicalDisk { get; }
    public IEnumerable<IEnumerable<Win32PropertyModel>> BaseBoard { get; }
    public IEnumerable<IEnumerable<Win32PropertyModel>> CPU { get; }
    public IEnumerable<IEnumerable<Win32PropertyModel>> Memory { get; }
}
