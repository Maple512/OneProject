namespace OneProject.Desktop;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OneProject.Desktop.Componets;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;
using OneProject.Desktop.Theme;
using OneProject.Desktop.Theme.Themes;
using OneProject.Desktop.ViewModels;

public partial class MainWindow : OPWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        NotificationManager.Initialization(P_NotificationBox);

        Title = nameof(OneProject);

        LoadSettings();

        LoadMenu();
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

        GlobalSettings.Instance.Theme.Background = ThemeManager.CurrentTheme.Main.ToString();
        GlobalSettings.Instance.Theme.IsLight = ThemeManager.CurrentTheme.IsLight;

        GlobalSettings.Instance.Save();

        base.OnClosing(e);
    }

    void LoadMenu()
    {
        ObservableCollection<MenuItemModel> menus =
        [
            MenuItemModel.Create<Home>("首页",IconKind.Home),
            MenuItemModel.Create<WindowsRegistry>("注册表",IconKind.Windows_Registy),
            MenuItemModel.Create<Monitor>("监控",IconKind.Monitor),
            MenuItemModel.Create<WindowsInfo>("系统",IconKind.Windows),
            MenuItemModel.Create<About>("关于",IconKind.About),
        ];

        DataContext = new MainWindowModel(menus);
    }

    void LoadSettings()
    {
        WindowState = GlobalSettings.Instance.Windows.MainWindowState;
        Height = GlobalSettings.Instance.Windows.MainWindowHeight;
        Width = GlobalSettings.Instance.Windows.MainWindowWidth;

        if(GlobalSettings.Instance.IsLoadFile)
        {
            Top = GlobalSettings.Instance.Windows.MainWindowTop;
            Left = GlobalSettings.Instance.Windows.MainWindowLeft;
        }
        else
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        var source = GlobalSettings.Instance.Windows.FontFamily;
        if(source != null || FontFamily.Source != source)
        {
            var font = FontHelper.SystemFonts.FirstOrDefault(x => x.Source == source);

            if(font != null)
            {
                FontFamily = font;
            }
        }
    }
}
