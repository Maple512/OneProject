namespace OneProject.Desktop.ViewModels;

using System.Collections.ObjectModel;

public partial class MainWindowModel : ObservableObject
{
    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private MenuItemModel selectedItem;

    public MainWindowModel(
        string title,
        string version,
        ObservableCollection<MenuItemModel> pages,
        int selectedIndex = 0)
    {
        Title = title;
        Version = version;
        Menus = pages;
        SelectedItem = pages[selectedIndex];
        SelectedIndex = selectedIndex;
    }

    public string Title { get; }

    public string Version { get; }

    public ObservableCollection<MenuItemModel> Menus { get; }
}
