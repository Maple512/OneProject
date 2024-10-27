namespace OneProject.Desktop.Theme.Converters;
using System;
using System.Linq;

public class FirstNonNullConverter : IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
        => values?.FirstOrDefault(v => v is not null);

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
