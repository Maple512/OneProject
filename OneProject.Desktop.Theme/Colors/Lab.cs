namespace OneProject.Desktop.Theme.Colors;

/// <summary>
/// </summary>
/// <param name="L" >亮度[0,100]</param>
/// <param name="A" >从红色到绿色的范围[-128,127]</param>
/// <param name="B" >从黄色到蓝色的范围[-128,127]</param>
internal record struct Lab(double L, double A, double B);

internal class LabConstants
{
    public const double Kn = 18;

    public const double WhitePointX = 0.95047;
    public const double WhitePointY = 1;
    public const double WhitePointZ = 1.08883;
    public const double k = 24389d / 27.0;
    public const double e = 216d / 24389.0;

    public static readonly double eCubedRoot = Math.Pow(e, 1.0 / 3d);
}

internal static class LabExtensions
{
    public static Lab ToLab(this Color c) => c.ToXyz().ToLab();

    public static Color ToColor(this Lab lab) => lab.ToXyz().ToColor();
}
