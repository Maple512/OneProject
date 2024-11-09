namespace OneProject.Desktop.ViewModels;

using System.Collections.Immutable;
using System.Linq;
using Humanizer;
using OneProject.Desktop.Components;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;

public partial class WindowsRegistryModel : ModelBase<WindowsRegistry>
{
    static string Folder = Path.Combine(GlobalSettings.TempPath, "RegBackupFiles");

    [ObservableProperty]
    private string? search;

    [ObservableProperty]
    private string? content;

    [ObservableProperty]
    private int? pointSequence;

    [ObservableProperty]
    private DateTime? lastQueryAt;
    private readonly List<SystemRestoreModel>? points = null;

    [ObservableProperty]
    private IEnumerable<FileInfo> _backupFiles = [];

    public WindowsRegistryModel()
    {
        Search = "使用 Visual Studio 打开";

        Dispatcher.BeginInvoke(QueryBackupFiles);
    }

    void QueryBackupFiles()
    {
        if(Directory.Exists(Folder) == false)
        {
            return;
        }

        BackupFiles = Directory.EnumerateFiles(Folder, "*.reg")
            .Select(x => new FileInfo(x)).ToImmutableList();
    }

    private List<SystemRestoreModel> GetPoints()
    {
        if(points is null)
        {
            Query();
        }

        return points!;
    }

    [RelayCommand]
    private async Task BackupAsync()
    {
        var watch = Stopwatch.StartNew();

        var file = FileHelper.TryGetFileInfo(Path.Combine(Folder, $"{DateTime.Now:yyyy_MM_dd_HH-mm}.reg"));

        await WindowsRegistryManager.ExportAllAsync(file);

        watch.Stop();

        ConfirmWindow.Open(new ConfirmModel($"已完成备份，用时：{watch.Elapsed.Humanize()}")
        {
            OkText = "打开目录",
            OkCommand = RegularCommand.OpenExplorer,
            OkCommandParameter = file.FullName,
        });

        QueryBackupFiles();
    }

    [RelayCommand]
    private void Query()
    {
        if(string.IsNullOrWhiteSpace(Search))
        {
            return;
        }

        //await Task.Run(() =>
        //{

        //    //var isAdmin = WindowsIdentityManager.IsCurrentUserAdministrator();

        //    //if(!isAdmin)
        //    //{
        //    //    WindowsIdentityManager.RequestAdministratorRights();
        //    //}

        //    var result = WindowsRegistryManager.Search(Search);

        //    Content = JsonSerializer.Serialize(result, JsonSerializerHelper.ToReadonlyJson);
        //});

        //result.Count.ShouldBeGreaterThan(0);

        //await Task.Run(() =>
        //{
        //    points = SystemRestoreManager.Query();

        //    Content = JsonSerializer.Serialize(points.OrderByDescending(x => x.SequenceNumber), JsonSerializerHelper.ToReadonlyJson);

        //    LastQueryAt = DateTime.Now;
        //});
    }

    [RelayCommand]
    private void Delete(bool isAll)
    {
        //var points = GetPoints();

        //if(isAll && points.Count != 0)
        //{
        //    foreach(var point in points)
        //    {
        //        SystemRestoreManager.Delete(point.SequenceNumber);
        //    }

        //    return;
        //}

        //if(!(PointSequence > 0) || !points.Any(x => x.SequenceNumber == PointSequence))
        //{
        //    return;
        //}

        //SystemRestoreManager.Delete((uint)PointSequence);
    }
}
