namespace OneProject.Desktop.Theme.Colors;

public class ColorPair(Color color, Color? foreground = null)
{
    public Color Background { get; } = color;

    public Color Foreground { get; } = foreground ?? color.ContrastingForegroundColor();
}
