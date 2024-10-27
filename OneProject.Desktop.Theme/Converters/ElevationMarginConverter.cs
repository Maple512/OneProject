namespace OneProject.Desktop.Theme.Converters;

using OneProject.Desktop.Theme.Assists;

public class ElevationMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is Elevation elevation && elevation != Elevation.Se0)
        {
            return new Thickness(ShadowElevationAssist.GetDropShadow(elevation)!.BlurRadius);
        }

        return new Thickness(0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class ElevationRadiusConverter : IValueConverter
{
    public double Multiplier { get; set; } = 1.0;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is Elevation elevation && elevation != Elevation.Se0)
        {
            return ShadowElevationAssist.GetDropShadow(elevation)!.BlurRadius * Multiplier;
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
