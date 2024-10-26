namespace OneProject.Desktop.ViewModels;

using System.Collections.ObjectModel;

public partial class MainWindowModel : ObservableObject
{
    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private MenuItemModel selectedItem;

    public MainWindowModel(ObservableCollection<MenuItemModel> pages)
    {
        Menus = pages;
        SelectedItem = pages[1];
    }

    public ObservableCollection<MenuItemModel> Menus { get; }
}
