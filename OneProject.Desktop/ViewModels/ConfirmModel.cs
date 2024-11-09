namespace OneProject.Desktop.ViewModels;

using OneProject.Desktop.Components;

public class ConfirmModel : ModelBase<ConfirmWindow>
{
    public string Title { get; init; } = "提示";

    public string Message { get; }

    public ConfirmModel(string message) => Message = Check.NotNullOrWhiteSpace(message);

    public bool ShowCancelButton { get; set; } = true;

    public string? CancelText { get; init; } = "取消";

    public ICommand? CancelCommand { get; set; }

    public string? OkText { get; init; } = "确定";

    public ICommand? OkCommand { get; set; }

    public object? OkCommandParameter { get; set; }
}
