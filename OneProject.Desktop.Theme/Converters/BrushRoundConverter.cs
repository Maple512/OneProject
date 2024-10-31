namespace OneProject.Desktop.Converters;

using OneProject.Desktop.Colors;

public class BrushRoundConverter : IValueConverter
{
    public Brush? HighValue { get; set; } = Brushes.White;

    public Brush? LowValue { get; set; } = Brushes.Black;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is not SolidColorBrush solidColorBrush)
        {
            return null;
        }

        return solidColorBrush.Color.IsLight()
            ? HighValue
            : LowValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
