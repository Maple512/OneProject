namespace OneProject.Desktop.ViewModels;

using System.Collections.ObjectModel;
using OneProject.Desktop.Pages;
using OneProject.Desktop.Themes;

public partial class MainWindowModel : ObservableObject
{
    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private PageItemModel selectedItem;

    public MainWindowModel(ObservableCollection<PageItemModel> pages)
    {
        Pages = pages;
        SelectedItem = pages[0];
    }

    public ObservableCollection<PageItemModel> Pages { get; }

    [RelayCommand]
    static void OpenAboutWindow()
    {
        var aboutWindow = ModelWindow.OpenModel(App.Current.MainWindow, "关于", new About());

        aboutWindow.ShowDialog();
    }
}
