namespace OneProject.Desktop.Converters;

using System;
using System.Globalization;

public enum ThicknessDirectionType
{
    Bottom,
    Top,
    Left,
    Right,
}

public class ThicknessDirectionConverter : IValueConverter
{
    public ThicknessDirectionType Direction { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is not double d)
        {
            return Binding.DoNothing;
        }

        return Direction switch
        {
            ThicknessDirectionType.Bottom => new Thickness(0, 0, 0, d),
            ThicknessDirectionType.Top => new Thickness(0, d, 0, 0),
            ThicknessDirectionType.Left => new Thickness(d, 0, 0, 0),
            ThicknessDirectionType.Right => new Thickness(0, 0, d, 0),
            _ => Binding.DoNothing,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
