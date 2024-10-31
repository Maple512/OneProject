namespace OneProject.Desktop.Pages;

using System.Windows.Controls;
using OneProject.Desktop.ViewModels;

/// <summary>
/// WindowsRightContentMenu.xaml 的交互逻辑
/// </summary>
public partial class RightMouseButton : UserControl
{
    public RightMouseButton()
    {
        InitializeComponent();

        DataContext = new RightMouseButtonModel();
    }
}
