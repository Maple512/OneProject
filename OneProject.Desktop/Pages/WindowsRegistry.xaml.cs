namespace OneProject.Desktop.Pages;

using System.Windows.Controls;
using OneProject.Desktop.ViewModels;

public partial class WindowsRegistry : UserControl
{
    public WindowsRegistry()
    {
        InitializeComponent();

        DataContext = new WindowsRegistryModel();
    }
}
