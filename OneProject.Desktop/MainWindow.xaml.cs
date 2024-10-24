namespace OneProject.Desktop;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;
using OneProject.Desktop.Theme;
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

        ThemeText.Text = ThemeManager.CurrentTheme.Color.ToString();
        ThemeChangeBtn.Content = ThemeManager.CurrentTheme.IsLight ? "亮色" : "暗色";

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

        GlobalSettings.Instance.Theme.Color = ThemeManager.CurrentTheme.Color.ToString();
        GlobalSettings.Instance.Theme.IsLight = ThemeManager.CurrentTheme.IsLight;

        GlobalSettings.Instance.Save();

        base.OnClosing(e);
    }

    private void OnThemeBtnClick(object sender, RoutedEventArgs e)
    {
        var color = (Color)ColorConverter.ConvertFromString(ThemeText.Text);

        ThemeManager.ChangeTheme(Application.Current, color);

        ThemeChangeBtn.Content = ThemeManager.CurrentTheme.IsLight ? "亮色" : "暗色";
    }
}
