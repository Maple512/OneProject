namespace OneProject.Desktop.Colors;

public class ColorPair(Color color, Color? foreground = null)
{
    public Color Background { get; } = color;

    public Color Foreground { get; } = foreground ?? color.ContrastingForegroundColor();
}
