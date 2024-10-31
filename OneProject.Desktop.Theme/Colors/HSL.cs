namespace OneProject.Desktop.Colors;

/// <summary>
///     HSL
///     <para>与<see cref="HSB" />的区别：HSL是一个倒圆锥模型，HSB是圆柱模型</para>
/// </summary>
/// <param name="Hue" >色调[0,360]</param>
/// <param name="Saturation" >饱和度[0,1]</param>
/// <param name="Lightness" >亮度[0,1]</param>
[DebuggerStepThrough]
public readonly record struct HSL
{
    public HSL(double hue, double saturation, double lightness)
    {
        Check.ThrowIfNotBetween(hue, 0, 360);
        Check.ThrowIfNotBetween(saturation, 0, 1);
        Check.ThrowIfNotBetween(lightness, 0, 1);

        Hue = hue;
        Saturation = saturation > 1 ? saturation / 100 : saturation;
        Lightness = lightness > 1 ? lightness / 100 : lightness;
    }

    public double Hue { get; }

    public double Saturation { get; }

    public double Lightness { get; }
}

public static class HSLExtensions
{
    public static HSL ToHSL(this Color color) => ToHSL(color.R, color.G, color.B);

    public static HSL ToHSL(byte red, byte green, byte blue)
    {
        double r, g, b, min, max, delta, h, s, l;

        r = red / 255.0;
        g = green / 255.0;
        b = blue / 255.0;

        min = new List<double> { r, g, b, }.Min();
        max = new List<double> { r, g, b, }.Max();
        delta = max - min;
        l = (min + max) / 2;

        if(delta == 0)
        {
            h = 0;
            s = 0;
        }
        else
        {
            s = l <= 0.5 ? delta / (min + max) : delta / (2 - max - min);

            if(r == max)
            {
                h = (g - b) / 6 / delta;
            }
            else if(g == max)
            {
                h = (1.0 / 3) + ((b - r) / 6 / delta);
            }
            else
            {
                h = (2.0 / 3) + ((r - g) / 6 / delta);
            }

            h = h < 0 ? ++h : h;
            h = h > 1 ? --h : h;
        }

        return new(h * 360, s, l);
    }

    public static HSB ToHSB(this HSL hsl)
    {
        double s, l, hsvS, hsvB;

        s = hsl.Saturation;
        l = hsl.Lightness;

        hsvB = l + (s * Math.Min(l, 1 - l));

        hsvS = hsvB == 0 ? 0 : 2 * (1 - (l / hsvB));

        return new(hsl.Hue, hsvS, hsvB);
    }

    public static Color ToColor(this HSL hsl, byte alpha = byte.MaxValue)
    {
        var h = hsl.Hue;
        var s = hsl.Saturation;
        var l = hsl.Lightness;

        var i = (int)(h / 60D);
        var circleSegmentFraction = (h - (60 * i)) / 60;

        var maxRGB = l < 0.5 ? l * (1 + s) : l + s - (l * s);
        var minRGB = (2 * l) - maxRGB;
        var delta = maxRGB - minRGB;

        return i switch
        {
            0 => ColorHelper.CreateColor(maxRGB, (delta * circleSegmentFraction) + minRGB, minRGB, alpha),
            1 => ColorHelper.CreateColor((delta * (1 - circleSegmentFraction)) + minRGB, maxRGB, minRGB, alpha),
            2 => ColorHelper.CreateColor(minRGB, maxRGB, (delta * circleSegmentFraction) + minRGB, alpha),
            3 => ColorHelper.CreateColor(minRGB, (delta * (1 - circleSegmentFraction)) + minRGB, maxRGB, alpha),
            4 => ColorHelper.CreateColor((delta * circleSegmentFraction) + minRGB, minRGB, maxRGB, alpha),
            _ => ColorHelper.CreateColor(maxRGB, minRGB, (delta * (1 - circleSegmentFraction)) + minRGB, alpha),
        };
    }
}
