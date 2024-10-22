namespace OneProject.Desktop;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;
using OneProject.Desktop.ViewModels;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        Title = nameof(OneProject);

        WindowState = GlobalSettings.Instance.Windows.MainWindowState;
        Height = GlobalSettings.Instance.Windows.MainWindowHeight;
        Width = GlobalSettings.Instance.Windows.MainWindowWidth;
        Top = GlobalSettings.Instance.Windows.MainWindowTop;
        Left = GlobalSettings.Instance.Windows.MainWindowLeft;

        var source = GlobalSettings.Instance.Windows.FontFamily;
        if(source != null || FontFamily.Source != source)
        {
            var font = FontHelper.SystemFonts.FirstOrDefault(x => x.Source == source);

            if(font != null)
            {
                FontFamily = font;
            }
        }

        ObservableCollection<MenuItemModel> menus =
        [
            MenuItemModel.Create<Home>("首页"),
            MenuItemModel.Create<WindowsReg>("注册表"),
        ];

        DataContext = new MainWindowModel("OneProject", GlobalSettings.Version, menus);
    }

    //private void OnSelectedItemChanged(object sender, DependencyPropertyChangedEventArgs e)
    //    => MainScrollViewer.ScrollToHome();

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
