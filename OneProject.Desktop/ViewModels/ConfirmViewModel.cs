namespace OneProject.Desktop.ViewModels;

public class ConfirmViewModel : ModelBase<ConfirmViewModel>
{
    public string Title { get; init; } = "提示";

    public string Message { get; }

    public ConfirmViewModel(string message) => Message = Check.NotNullOrWhiteSpace(message);

    public string? CancelText { get; init; } = "取消";

    public ICommand? CancelCommand { get; set; }

    public string? OkText { get; init; } = "确定";

    public ICommand? OkCommand { get; set; }

    public object? OkCommandParameter { get; set; }
}
