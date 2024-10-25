namespace OneProject.Desktop.Theme.Converters;

using System;
using System.Windows.Media.Effects;
using OneProject.Desktop.Theme.Assists;

public class ShadowElevationConverter : IValueConverter
{
    public static readonly ShadowElevationConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value switch
        {
            Elevation elevation => Clone(Convert(elevation)),
            _ => null,
        };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();

    public static DropShadowEffect? Convert(Elevation elevation) => ShadowElevationAssist.GetDropShadow(elevation);

    private static DropShadowEffect? Clone(DropShadowEffect? dropShadowEffect)
    {
        if(dropShadowEffect is null)
        {
            return null;
        }

        return new()
        {
            BlurRadius = dropShadowEffect.BlurRadius,
            Color = dropShadowEffect.Color,
            Direction = dropShadowEffect.Direction,
            Opacity = dropShadowEffect.Opacity,
            RenderingBias = dropShadowEffect.RenderingBias,
            ShadowDepth = dropShadowEffect.ShadowDepth,
        };
    }
}
