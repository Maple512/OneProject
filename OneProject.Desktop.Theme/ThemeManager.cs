namespace OneProject.Desktop.Theme;

public static class ThemeManager
{
    private static int _index = 0;

    public static DesktopTheme CurrentTheme { get; private set; } = [];

    public static void Initialize(Application app, string? color, bool isLight)
    {
        if(app.Resources.MergedDictionaries.FirstOrDefault(x => x is IThemeResourceDictionary) is not DesktopTheme resource)
        {
            return;
        }

        _index = app.Resources.MergedDictionaries.IndexOf(resource);

        CurrentTheme = resource;

        if(!string.IsNullOrEmpty(color))
        {
            ChangeTheme(app, (Color)ColorConverter.ConvertFromString(color), isLight);
        }
    }

    public static void ChangeTheme(Application app, Color parmaryColor, bool isLight = true)
    {
        app.Resources.MergedDictionaries.RemoveAt(_index);

        CurrentTheme.Color = parmaryColor;
        CurrentTheme.IsLight = isLight;

        app.Resources.MergedDictionaries.Insert(_index, CurrentTheme);
    }
}
