namespace OneProject.Desktop.Colors;

internal record struct Xyz(double X, double Y, double Z);

internal static class XyzExtensions
{
    public static Color ToColor(this Xyz xyz)
    {
        var r = XYZToRgb((3.2404542 * xyz.X) - (1.5371385 * xyz.Y) - (0.4985314 * xyz.Z));
        var g = XYZToRgb((-0.9692660 * xyz.X) + (1.8760108 * xyz.Y) + (0.0415560 * xyz.Z));
        var b = XYZToRgb((0.0556434 * xyz.X) - (0.2040259 * xyz.Y) + (1.0572252 * xyz.Z));

        return Color.FromRgb(RgbClip(r), RgbClip(g), RgbClip(b));
    }

    public static Lab ToLab(this Xyz xyz)
    {
        static double xyz_lab(double v)
        {
            if(v > LabConstants.e)
            {
                return Math.Pow(v, 1 / 3.0);
            }

            return ((v * LabConstants.k) + 16) / 116;
        }

        var fx = xyz_lab(xyz.X / LabConstants.WhitePointX);
        var fy = xyz_lab(xyz.Y / LabConstants.WhitePointY);
        var fz = xyz_lab(xyz.Z / LabConstants.WhitePointZ);

        var l = (116 * fy) - 16;
        var a = 500 * (fx - fy);
        var b = 200 * (fy - fz);

        return new(l, a, b);
    }

    private static double XYZToRgb(double d)
    {
        if(d > 0.0031308)
        {
            return 255.0 * ((1.055 * Math.Pow(d, 1.0 / 2.4)) - 0.055);
        }

        return 255.0 * (12.92 * d);
    }

    private static byte RgbClip(double d)
    {
        if(d < 0)
        {
            return 0;
        }

        if(d > 255)
        {
            return 255;
        }

        return (byte)Math.Round(d);
    }

    public static Xyz ToXyz(this Color c)
    {
        var r = RgbToXYZ(c.R);
        var g = RgbToXYZ(c.G);
        var b = RgbToXYZ(c.B);

        var x = (0.4124564 * r) + (0.3575761 * g) + (0.1804375 * b);
        var y = (0.2126729 * r) + (0.7151522 * g) + (0.0721750 * b);
        var z = (0.0193339 * r) + (0.1191920 * g) + (0.9503041 * b);

        return new(x, y, z);
    }

    private static double RgbToXYZ(double v)
    {
        v /= 255d;
        if(v > 0.04045)
        {
            return Math.Pow((v + 0.055) / 1.055, 2.4);
        }

        return v / 12.92;
    }

    public static Xyz ToXyz(this Lab lab)
    {
        var y = (lab.L + 16.0) / 116.0;
        var x = double.IsNaN(lab.A) ? y : y + (lab.A / 500.0);
        var z = double.IsNaN(lab.B) ? y : y - (lab.B / 200.0);

        y = LabConstants.WhitePointY * LabToXYZ(y);
        x = LabConstants.WhitePointX * LabToXYZ(x);
        z = LabConstants.WhitePointZ * LabToXYZ(z);

        return new(x, y, z);
    }

    private static double LabToXYZ(double d)
    {
        if(d > LabConstants.eCubedRoot)
        {
            return d * d * d;
        }

        return ((116d * d) - 16d) / LabConstants.k;
    }
}
