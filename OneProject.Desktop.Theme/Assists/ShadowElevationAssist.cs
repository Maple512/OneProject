namespace OneProject.Desktop.Assists;

using System;
using System.Collections.Generic;
using System.Windows.Media.Effects;

public enum Elevation
{
    Se0,
    Se1,
    Se2,
    Se3,
    Se4,
    Se5,
    Se6,
    Se7,
    Se8,
    Se12,
    Se16,
    Se24,
}

public class ShadowElevationAssist
{
    public static readonly DependencyProperty ElevationProperty =
        DependencyProperty.RegisterAttached(
            nameof(Elevation),
            typeof(Elevation),
            typeof(ShadowElevationAssist),
            new FrameworkPropertyMetadata(default(Elevation), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetElevation(DependencyObject element, Elevation value)
        => element.SetValue(ElevationProperty, value);

    public static Elevation GetElevation(DependencyObject element) => (Elevation)element.GetValue(ElevationProperty);

    public static DropShadowEffect? GetDropShadow(Elevation elevation)
        => ShadowElevationInfo.GetShadowEffect(elevation);
}

internal static class ShadowElevationInfo
{
    private static readonly Dictionary<Elevation, DropShadowEffect?> _container;

    static ShadowElevationInfo()
    {
        const string uri = "pack://application:,,,/OneProject.Desktop.Theme;component/Themes/Shadows.xaml";
        var resources = new ResourceDictionary { Source = new(uri, UriKind.Absolute), };

        _container = new Dictionary<Elevation, DropShadowEffect?>()
        {
            { Elevation.Se1, resources["Shadow1"] as DropShadowEffect },
            { Elevation.Se2, resources["Shadow2"] as DropShadowEffect },
            { Elevation.Se3, resources["Shadow3"] as DropShadowEffect },
            { Elevation.Se4, resources["Shadow4"] as DropShadowEffect },
            { Elevation.Se5, resources["Shadow5"] as DropShadowEffect },
            { Elevation.Se6, resources["Shadow6"] as DropShadowEffect },
            { Elevation.Se7, resources["Shadow7"] as DropShadowEffect },
            { Elevation.Se8, resources["Shadow8"] as DropShadowEffect },
            { Elevation.Se12, resources["Shadow12"] as DropShadowEffect },
            { Elevation.Se16, resources["Shadow16"] as DropShadowEffect },
            { Elevation.Se24, resources["Shadow24"] as DropShadowEffect },
        };
    }

    public static DropShadowEffect? GetShadowEffect(Elevation elevation)
    {
        if(elevation is Elevation.Se0)
        {
            return null;
        }

        if(_container.TryGetValue(elevation, out var effect))
        {
            return effect;
        }

        return null;
    }
}
