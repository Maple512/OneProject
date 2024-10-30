namespace OneProject.Desktop.Theme.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

public class DoubleToGridLengthConverter : IValueConverter
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
