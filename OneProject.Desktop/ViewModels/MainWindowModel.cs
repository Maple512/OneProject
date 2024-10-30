namespace OneProject.Desktop.ViewModels;

using System.Collections.ObjectModel;
using OneProject.Desktop.Pages;
using OneProject.Desktop.Theme.Themes;

public partial class MainWindowModel : ObservableObject
{
    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private MenuItemModel selectedItem;

    public MainWindowModel(ObservableCollection<MenuItemModel> pages)
    {
        Menus = pages;
        SelectedItem = pages[0];
    }

    public ObservableCollection<MenuItemModel> Menus { get; }

    [RelayCommand]
    static void OpenAboutWindow()
    {
        var aboutWindow = ModelWindow.OpenModel(App.Current.MainWindow, "关于", new About());

        aboutWindow.ShowDialog();
    }
}
