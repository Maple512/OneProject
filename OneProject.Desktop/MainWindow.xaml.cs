namespace OneProject.Desktop;

using System.ComponentModel;
using System.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        GlobalSettings.Instance.Windows.MainWindowState = WindowState;
        GlobalSettings.Instance.Windows.MainWindowHeight = Height;
        GlobalSettings.Instance.Windows.MainWindowWidth = Width;
        GlobalSettings.Instance.Windows.MainWindowTop = Top;
        GlobalSettings.Instance.Windows.MainWindowLeft = Left;
        GlobalSettings.Instance.Windows.FontFamily = FontFamily.Source;

        GlobalSettings.Instance.Save();

        base.OnClosing(e);
    }
}
