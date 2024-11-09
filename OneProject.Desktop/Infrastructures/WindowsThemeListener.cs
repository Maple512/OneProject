namespace OneProject.Desktop.Infrastructures;

using Microsoft.Win32;

using OneProject.Desktop.Win32Native;

public static class WindowsThemeListener
{
    static UserPreferenceChangingEventHandler? handler;

    public static void Listen(Application app)
    {
        handler = (object sender, UserPreferenceChangingEventArgs e) =>
        {
            UpdateAccentColor(app);
        };

        SystemEvents.UserPreferenceChanging += handler;
    }

    public static void Unlisen()
    {
        SystemEvents.UserPreferenceChanging -= handler;
    }

    public static void UpdateAccentColor(Application app)
    {
        ThemeManager.ChangeTheme(app, WindowsThemeManger.GetCurrentThemeColor());
    }
}
