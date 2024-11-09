namespace OneProject.Desktop;

using OneProject.Desktop.Colors;

public class DesktopTheme : ResourceDictionary, IThemeResourceDictionary
{
    private static Color _light = (Color)ColorConverter.ConvertFromString("#F1F1F1");
    private static Color _dark = (Color)ColorConverter.ConvertFromString("#323232");

    public bool IsLight { get; set; }
    public Color Main { get; set; }
    public Color A { get; set; }
    public Color B { get; set; }

    public static DesktopTheme Initialize(ResourceDictionary resource)
    {
        var theme = (DesktopTheme)resource;

        theme.Main = (Color)resource["Color.Main"];
        theme.A = (Color)resource["Color.A"];
        theme.B = (Color)resource["Color.B"];
        theme.IsLight = theme.A.IsLight();

        return theme;
    }

    public void Change(Color color, bool isLight = true)
    {
        if(Main != color)
        {
            Main = color;

            this["Color.Main"] = Main;
        }

        if(IsLight != isLight)
        {
            IsLight = isLight;
            A = IsLight ? _light : _dark;
            B = IsLight ? _dark : _light;

            this["Color.A"] = A;
            this["Color.B"] = B;
        }
    }
}
