namespace OneProject.Desktop.Theme;

public class DesktopTheme : ResourceDictionary, IThemeResourceDictionary
{
    private static Color _light = (Color)ColorConverter.ConvertFromString("#F1F1F1");
    private static Color _dark = (Color)ColorConverter.ConvertFromString("#323232");
    private Color _primary;
    private bool _isLight;

    public Color Color
    {
        get => _primary;
        set
        {
            if(_primary == value)
            {
                return;
            }

            _primary = value;
            this["Brush.Color"] = new SolidColorBrush(_primary);
        }
    }

    public bool IsLight
    {
        get => _isLight; set
        {
            if(_isLight == value)
            {
                return;
            }

            _isLight = value;
            if(_isLight)
            {
                this["Brush.Background"] = new SolidColorBrush(_light);
                this["Brush.Foreground"] = new SolidColorBrush(_dark);
            }
            else
            {
                this["Brush.Background"] = new SolidColorBrush(_dark);
                this["Brush.Foreground"] = new SolidColorBrush(_light);
            }
        }
    }

    public Color Background => IsLight ? _light : _dark;

    public Color Foreground => IsLight ? _dark : _light;
}
