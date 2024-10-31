namespace OneProject.Desktop.ViewModels;

using OneProject.Desktop.Pages;
using OneProject.Desktop.Themes;

public partial class RightMouseButtonModel : ModelBase<RightMouseButton>
{
    [RelayCommand]
    private static void AddMenuInFileContext()
    {
        var window = ModelWindow.OpenModel(App.Current.MainWindow, "添加右键菜单项", new AddContextMenu());

        window.ShowDialog();
    }
}
