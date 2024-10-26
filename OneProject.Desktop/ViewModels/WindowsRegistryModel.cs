namespace OneProject.Desktop.ViewModels;

using System.Linq;
using System.Text.Json;
using OneProject.WindowsManagment;

public partial class WindowsRegistryModel : ModelBase<WindowsRegistryModel>
{
    [ObservableProperty]
    private string? content;

    [ObservableProperty]
    private int? pointSequence;

    [ObservableProperty]
    private DateTime? lastQueryAt;
    private List<SystemRestoreModel>? points = null;

    public WindowsRegistryModel()
    {
    }

    private async Task<List<SystemRestoreModel>> GetPointsAsync()
    {
        if(points is null)
        {
            await QueryAsync();
        }

        return points!;
    }

    [RelayCommand]
    private static async Task BackupAsync()
    {
        await Task.Run(() => SystemRestoreManager.Create());
    }

    [RelayCommand]
    private async Task QueryAsync()
    {
        await Task.Run(() =>
        {
            points = SystemRestoreManager.Query();

            Content = JsonSerializer.Serialize(points.OrderByDescending(x => x.SequenceNumber), JsonSerializerHelper.ToReadonlyJson);

            LastQueryAt = DateTime.Now;
        });
    }

    [RelayCommand]
    private async Task DeleteAsync(bool isAll)
    {
        var points = await GetPointsAsync();

        if(isAll && points.Count != 0)
        {
            foreach(var point in points)
            {
                SystemRestoreManager.Delete(point.SequenceNumber);
            }

            return;
        }

        if(!(PointSequence > 0) || !points.Any(x => x.SequenceNumber == PointSequence))
        {
            return;
        }

        SystemRestoreManager.Delete((uint)PointSequence);
    }
}
