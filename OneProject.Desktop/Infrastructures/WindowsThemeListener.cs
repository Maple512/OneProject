namespace OneProject.Desktop.Infrastructures;

using Microsoft.Win32;
using OneProject.Desktop.Theme;
using OneProject.Desktop.Win32Native;

/// <summary>
/// Windows系统主题监听
/// </summary>
public class WindowsThemeListener
{
    public static void Listen(Application app)
    {
        SystemEvents.UserPreferenceChanging += OnUserPreferenceChanging;

        void OnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
        {
            UpdateAccentColor(app);
        }
    }

    public static void UpdateAccentColor(Application app)
    {
        ThemeManager.ChangeTheme(app, WindowsThemeManger.GetCurrentThemeColor());
    }
}
