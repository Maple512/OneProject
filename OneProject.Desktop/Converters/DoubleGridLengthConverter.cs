namespace OneProject.Desktop.Converters;

using System;
using System.Globalization;

public class DoubleGridLengthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is double d)
        {
            return new GridLength(d);
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
