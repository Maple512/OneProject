namespace OneProject.Desktop.Colors;

public static class ColorHelper
{
    public static Color CreateColor(double r, double g, double b, byte alpha = byte.MaxValue)
        => Color.FromArgb(alpha, (byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
}
