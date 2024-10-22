namespace OneProject.Desktop.Theme.Colors;

public class ColorGenerator
{
    private readonly Lazy<IEnumerable<KeyValuePair<int, double>>> _darkColorMap = new(DarkColorMapCreator);

    /// <summary>
    ///     色阶
    /// </summary>
    public int HUEStep { get; init; } = 2;

    /// <summary>
    ///     饱和度阶梯，浅色部分
    /// </summary>
    public double LightSaturationStep { get; init; } = 0.16;

    /// <summary>
    ///     饱和度阶梯，深色部分
    /// </summary>
    public double DarkSaturationStep { get; init; } = 0.05;

    /// <summary>
    ///     亮度阶梯，浅色部分
    /// </summary>
    public double LightBrightnessStep { get; init; } = 0.05;

    /// <summary>
    ///     亮度阶梯，深色部分
    /// </summary>
    public double DarkBrightnessStep { get; init; } = 0.15;

    /// <summary>
    ///     浅色数量，主色上
    /// </summary>
    public int LightColorCount { get; init; } = 5;

    /// <summary>
    ///     深色数量，主色下
    /// </summary>
    public int DarkColorCount { get; init; } = 5;

    public IEnumerable<ColorPair> Generate(Color color, bool isDark = false, Color? background = null)
    {
        var result = new List<Color>(10);
        for(var i = LightColorCount; i > 0; i--)
        {
            var hsv = color.ToHSB();

            // 色调
            var h = GetHue(hsv, i, true);
            // 饱和度
            var s = GetSaturation(hsv, i, true);
            // 亮度
            var b = GetBrightness(hsv, i, true);

            var hsb = new HSB(h, s, b);

            result.Add(hsb.ToColor());
        }

        for(var i = 1; i <= DarkColorCount; i++)
        {
            var hsv = color.ToHSB();

            // 色调
            var h = GetHue(hsv, i, false);
            // 饱和度
            var s = GetSaturation(hsv, i, false);
            // 亮度
            var b = GetBrightness(hsv, i, false);

            try
            {
                var hsb = new HSB(h, s, b);

                result.Add(hsb.ToColor());
            }
            catch { }
        }

        if(isDark)
        {
            background ??= SysColors.Black;

            result = _darkColorMap.Value.Select((x, i) => Min(result[x.Key], background.Value, x.Value)).ToList();
        }

        return result.Select(x => new ColorPair(x));
    }

    private static Color Min(Color color1, Color color2, double amount)
    {
        var r = ((color2.R - color1.R) * amount) + color1.R;

        var g = ((color2.G - color1.G) * amount) + color1.G;

        var b = ((color2.B - color1.B) * amount) + color1.B;

        return Color.FromRgb((byte)Math.Round(r), (byte)Math.Round(g), (byte)Math.Round(b));
    }

    /// <summary>
    ///     饱和度
    /// </summary>
    /// <returns></returns>
    public double GetSaturation(HSB hsb, int i, bool isLight)
    {
        if(hsb.Hue is 0 && hsb.Saturation is 0)
        {
            return hsb.Saturation;
        }

        double saturation;

        if(isLight)
        {
            saturation = hsb.Saturation - (LightSaturationStep * i);
        }
        else if(i == DarkColorCount)
        {
            saturation = hsb.Saturation + LightSaturationStep;
        }
        else
        {
            saturation = hsb.Saturation + (DarkSaturationStep * i);
        }

        // 修正边界值
        if(saturation > 1)
        {
            saturation = 1;
        }

        if(isLight && i == LightColorCount && saturation > 0.1)
        {
            saturation = 0.1;
        }

        if(saturation < 0.06)
        {
            saturation = 0.06;
        }

        return saturation;
    }

    /// <summary>
    ///     色调
    /// </summary>
    /// <returns></returns>
    public double GetHue(HSB hsb, int index, bool isLight)
    {
        double hue;
        if(hsb.Hue is >= 60 and <= 240)
        {
            hue = isLight ? hsb.Hue - (HUEStep * index) : hsb.Hue + (HUEStep * index);
        }
        else
        {
            hue = isLight ? hsb.Hue + (HUEStep * index) : hsb.Hue - (HUEStep * index);
        }

        if(hue < 0)
        {
            hue += 360;
        }
        else if(hue >= 360)
        {
            hue -= 360;
        }

        return hue;
    }

    /// <summary>
    ///     亮度
    /// </summary>
    /// <returns></returns>
    public double GetBrightness(HSB hsb, int index, bool isLight)
    {
        double brightness;
        if(isLight)
        {
            brightness = hsb.Brightness + (LightBrightnessStep * index);
        }
        else
        {
            brightness = hsb.Brightness - (DarkBrightnessStep * index);
        }

        if(brightness > 1)
        {
            brightness = 1;
        }

        return brightness;
    }

    private static IEnumerable<KeyValuePair<int, double>> DarkColorMapCreator()
        =>
        [
            new(7, 0.15),
            new(6, 0.25),
            new(5, 0.3),
            new(5, 0.45),
            new(5, 0.65),
            new(5, 0.85),
            new(4, 0.9),
            new(3, 0.95),
            new(2, 0.97),
            new(1, 0.98),
        ];
}
