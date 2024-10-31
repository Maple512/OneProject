namespace OneProject.Desktop.Colors;

public static class ColorExtensions
{
    /// <summary>
    ///     相对亮度
    /// </summary>
    /// <remarks>
    ///     The relative brightness of any point in a color space, normalized to 0 for darkest black and 1 for lightest white
    ///     For the sRGB color space, the relative luminance of a color is defined as L = 0.2126 * R + 0.7152 * G + 0.0722 * B
    ///     where R, G and B are defined as:
    ///     <para><![CDATA[if RsRGB <= 0.03928 then R = RsRGB / 12.92 else R = ((RsRGB+0.055)/1.055) ^ 2.4]]></para>
    ///     <para><![CDATA[if GsRGB <= 0.03928 then G = GsRGB / 12.92 else G = ((GsRGB+0.055)/1.055) ^ 2.4]]></para>
    ///     <para><![CDATA[if BsRGB <= 0.03928 then B = BsRGB / 12.92 else B = ((BsRGB+0.055)/1.055) ^ 2.4]]></para>
    ///     and RsRGB, GsRGB, and BsRGB are defined as:
    ///     <para>RsRGB = R8bit/255</para>
    ///     <para>GsRGB = G8bit/255</para>
    ///     <para>BsRGB = B8bit/255</para>
    ///     Based on https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
    /// </remarks>
    /// <param name="color" ></param>
    /// <returns></returns>
    public static float RelativeLuminance(this Color color)
    {
        return
            (0.2126f * Cal(color.R / 255f)) +
            (0.7152f * Cal(color.G / 255f)) +
            (0.0722f * Cal(color.B / 255f));

        static float Cal(float colorValue)
        {
            return colorValue <= 0.03928f ? colorValue / 12.92f : (float)Math.Pow((colorValue + 0.055f) / 1.055f, 2.4);
        }
    }

    /// <summary>
    ///     对比度计算为（L1+0.05）/（L2+0.05），其中L1是较浅颜色的相对亮度，L2是较深颜色的相对照度。
    ///     Based on https://www.w3.org/TR/2008/REC-WCAG20-20081211/#contrast%20ratio
    /// </summary>
    /// <param name="color" ></param>
    /// <param name="color2" ></param>
    /// <returns></returns>
    public static float ContrastRatio(this Color color, Color color2)
    {
        var l1 = color.RelativeLuminance();
        var l2 = color2.RelativeLuminance();
        if(l2 > l1)
        {
            (l2, l1) = (l1, l2);
        }

        return (l1 + 0.05f) / (l2 + 0.05f);
    }

    /// <summary>
    ///     调整前景色，使其具有可接受的对比度。
    /// </summary>
    /// <param name="foreground" >The foreground color</param>
    /// <param name="background" >The background color</param>
    /// <param name="targetRatio" >The target contrast ratio</param>
    /// <param name="tolerance" >The tolerance to the contrast ratio needs to be within</param>
    /// <returns>The updated foreground color with the target contrast ratio with the background</returns>
    public static Color EnsureContrastRatio(
        this Color foreground,
        Color background,
        float targetRatio,
        float tolerance = 0.1f)
        => foreground.EnsureContrastRatio(background, targetRatio, out _, tolerance);

    /// <summary>
    ///     调整前景色，使其具有可接受的对比度。
    /// </summary>
    /// <param name="foreground" >The foreground color</param>
    /// <param name="background" >The background color</param>
    /// <param name="targetRatio" >The target contrast ratio</param>
    /// <param name="offset" >The offset that was applied</param>
    /// <param name="tolerance" >The tolerance to the contrast ratio needs to be within</param>
    /// <returns>The updated foreground color with the target contrast ratio with the background</returns>
    public static Color EnsureContrastRatio(
        this Color foreground,
        Color background,
        float targetRatio,
        out double offset,
        float tolerance = 0.1f)
    {
        offset = 0.0f;
        var ratio = foreground.ContrastRatio(background);
        if(ratio > targetRatio)
        {
            return foreground;
        }

        var contrastWithWhite = background.ContrastRatio(SysColors.White);
        var contrastWithBlack = background.ContrastRatio(SysColors.Black);

        var shouldDarken = contrastWithBlack > contrastWithWhite;

        //Lighten is negative
        var finalColor = foreground;
        double? adjust = null;

        while((ratio < targetRatio - tolerance || ratio > targetRatio + tolerance) &&
              finalColor != SysColors.White &&
              finalColor != SysColors.Black)
        {
            if(ratio - targetRatio < 0F)
            {
                //Move offset of foreground further away from background
                if(shouldDarken)
                {
                    if(adjust < 0d)
                    {
                        adjust /= -2d;
                    }
                    else
                    {
                        adjust ??= 1.0f;
                    }
                }
                else
                {
                    if(adjust > 0d)
                    {
                        adjust /= -2d;
                    }
                    else
                    {
                        adjust ??= -1.0f;
                    }
                }
            }
            else
            {
                //Move offset of foreground closer to background
                if(shouldDarken)
                {
                    if(adjust > 0d)
                    {
                        adjust /= -2d;
                    }
                    else
                    {
                        adjust ??= -1.0f;
                    }
                }
                else
                {
                    if(adjust < 0d)
                    {
                        adjust /= -2d;
                    }
                    else
                    {
                        adjust ??= 1.0f;
                    }
                }
            }

            offset += adjust.Value;

            finalColor = foreground.ShiftLightness(offset);

            ratio = finalColor.ContrastRatio(background);
        }

        return finalColor;
    }

    /// <summary>
    ///     偏移亮度
    /// </summary>
    /// <param name="color" ></param>
    /// <param name="amount" ></param>
    /// <returns></returns>
    public static Color ShiftLightness(this Color color, double amount = 1.0f)
    {
        var lab = color.ToLab();
        var shifted = new Lab(lab.L - (LabConstants.Kn * amount), lab.A, lab.B);
        return shifted.ToColor();
    }

    /// <summary>
    ///     偏移亮度
    /// </summary>
    /// <param name="color" ></param>
    /// <param name="amount" ></param>
    /// <returns></returns>
    public static Color ShiftLightness(this Color color, int amount = 1)
    {
        var lab = color.ToLab();

        var shifted = new Lab(lab.L - (LabConstants.Kn * amount), lab.A, lab.B);

        return shifted.ToColor();
    }

    public static Color Darken(this Color color, int amount = 1) => color.ShiftLightness(amount);

    public static Color Lighten(this Color color, int amount = 1) => color.ShiftLightness(-amount);

    public static Color ContrastingForegroundColor(this Color color)
        => color.IsLight() ? SysColors.Black : SysColors.White;

    public static bool IsLight(this Color color)
    {
        static double rgb_srgb(double d)
        {
            d /= 255.0;
            return d > 0.03928
                ? Math.Pow((d + 0.055) / 1.055, 2.4)
                : d / 12.92;
        }

        var r = rgb_srgb(color.R);
        var g = rgb_srgb(color.G);
        var b = rgb_srgb(color.B);

        var luminance = (0.2126 * r) + (0.7152 * g) + (0.0722 * b);

        return luminance > 0.179;
    }

    public static bool IsDark(this Color color) => !color.IsLight();
}
