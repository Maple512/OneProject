namespace OneProject.Desktop.Pages;

using System.Windows.Controls;
using OneProject.Desktop.ViewModels;

/// <summary>
/// System.xaml 的交互逻辑
/// </summary>
public partial class WindowsInfo : UserControl
{
    public WindowsInfo()
    {
        DataContext = new WindowsInfoModel();

        InitializeComponent();
    }
}
