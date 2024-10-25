namespace OneProject.Desktop.Theme;

using OneProject.Desktop.Theme.Colors;

public class DesktopTheme : ResourceDictionary, IThemeResourceDictionary
{
    private static Color _light = (Color)ColorConverter.ConvertFromString("#F1F1F1");
    private static Color _dark = (Color)ColorConverter.ConvertFromString("#323232");

    public bool IsLight { get; set; }
    public Color Background { get; set; }
    public Color Foreground { get; set; }
    public Color WindowBackground { get; set; }
    public Color WindowForeground { get; set; }

    public static DesktopTheme Initialize(ResourceDictionary resource)
    {
        var theme = (DesktopTheme)resource;

        theme.Foreground = ((SolidColorBrush)resource["Brush.Foreground"]).Color;
        theme.Background = ((SolidColorBrush)resource["Brush.Background"]).Color;
        theme.WindowForeground = ((SolidColorBrush)resource["Brush.Window.Foreground"]).Color;
        theme.WindowBackground = ((SolidColorBrush)resource["Brush.Window.Background"]).Color;
        theme.IsLight = theme.Foreground.IsLight();

        return theme;
    }

    public void Change(Color background, bool isLight = true)
    {
        if(Background != background)
        {
            Background = background;

            this["Brush.Background"] = new SolidColorBrush(Background);
        }

        if(IsLight != isLight)
        {
            IsLight = isLight;
            Foreground = IsLight ? _light : _dark;
            WindowForeground = IsLight ? _light : _dark;
            WindowBackground = IsLight ? _dark : _light;

            this["Brush.Foreground"] = new SolidColorBrush(Foreground);
            this["Brush.Window.Foreground"] = new SolidColorBrush(WindowForeground);
            this["Brush.Window.Background"] = new SolidColorBrush(WindowBackground);
        }
    }
}
