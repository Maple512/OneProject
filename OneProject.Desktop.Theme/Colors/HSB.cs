namespace OneProject.Desktop.Colors;

/// <summary>
///     HSB
///     <para>与<see cref="HSL" />的区别：HSL是一个倒圆锥模型，HSB是圆柱模型</para>
/// </summary>
/// <param name="Hue" >色调[0,360]</param>
/// <param name="Saturation" >饱和度[0,1]</param>
/// <param name="Brightness" >亮度[0,1]</param>
[DebuggerStepThrough]
public readonly record struct HSB
{
    public HSB(double hue, double saturation, double brightness)
    {
        Check.ThrowIfNotBetween(hue, 0, 360);
        Check.ThrowIfNotBetween(saturation, 0, 1);
        Check.ThrowIfNotBetween(brightness, 0, 1);

        Hue = hue;
        Saturation = saturation > 1 ? saturation / 100 : saturation;
        Brightness = brightness > 1 ? brightness / 100 : brightness;
    }

    public double Hue { get; }

    public double Saturation { get; }

    public double Brightness { get; }

    public override string ToString()
        => $"{Math.Round(Hue)}, {Math.Round(Saturation * 100D)}%, {Math.Round(Brightness * 100D)}%";
}

public static class HsbExtensions
{
    public static Color ToColor(this HSB hsb, byte alpha = byte.MaxValue)
    {
        var s = hsb.Saturation;
        var b = hsb.Brightness;

        if(s == 0)
        {
            // achromatic (grey)
            return ColorHelper.CreateColor(b, b, b, alpha);
        }

        var h = hsb.Hue;

        if(h >= 360.0)
        {
            h = 0;
        }

        h /= 60;
        var i = (int)h;
        var f = h - i;
        var p = b * (1 - s);
        var q = b * (1 - (s * f));
        var t = b * (1 - (s * (1 - f)));

        return i switch
        {
            0 => ColorHelper.CreateColor(b, t, p, alpha),
            1 => ColorHelper.CreateColor(q, b, p, alpha),
            2 => ColorHelper.CreateColor(p, b, t, alpha),
            3 => ColorHelper.CreateColor(p, q, b, alpha),
            4 => ColorHelper.CreateColor(t, p, b, alpha),
            _ => ColorHelper.CreateColor(b, p, q, alpha),
        };
    }

    public static HSB ToHSB(this Color color) => ToHSB(color.R, color.G, color.B);

    public static HSB ToHSB(byte red, byte green, byte blue)
    {
        double h, s, b;
        var num = red / 255.0;
        var num2 = green / 255.0;
        var num3 = blue / 255.0;
        var num4 = Math.Min(num, Math.Min(num2, num3));
        var num5 = Math.Max(num, Math.Max(num2, num3));
        b = num5;
        var num6 = num5 - num4;
        if(b == 0.0)
        {
            s = 0.0;
        }
        else
        {
            s = num6 / num5;
        }

        if(s == 0.0)
        {
            h = 0.0;
        }
        else if(num == num5)
        {
            h = (num2 - num3) / num6;
        }
        else if(num2 == num5)
        {
            h = 2.0 + ((num3 - num) / num6);
        }
        else
        {
            h = 4.0 + ((num - num2) / num6);
        }

        h *= 60.0;
        if(h < 0.0)
        {
            h += 360.0;
        }

        return new(h, s, b);
    }

    public static HSL ToHSL(this HSB hsb)
    {
        var s = hsb.Saturation;
        var b = hsb.Brightness;

        var hsl_l = b * (1 - (s / 2));
        double hsl_s;
        if(hsl_l is 0 or 1)
        {
            hsl_s = -1;
        }
        else
        {
            hsl_s = (b - hsl_l) / Math.Min(hsl_l, 1 - hsl_l);
        }

        return new(hsb.Hue, hsl_s, hsl_l);
    }
}
