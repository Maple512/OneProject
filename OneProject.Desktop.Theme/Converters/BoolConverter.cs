namespace OneProject.Desktop.Theme.Converters;

using System;
using System.Collections.Generic;

public class BoolConverter<T>(T trueValue, T falseValue) : IValueConverter
{
    public T TrueValue { get; set; } = trueValue;
    public T FalseValue { get; set; } = falseValue;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b
           && b
            ? TrueValue
            : FalseValue;
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is T t
           && EqualityComparer<T>.Default.Equals(t, TrueValue);
}

public class TrueToVisibleConverter() : BoolConverter<Visibility>(Visibility.Visible, Visibility.Collapsed);

public class FalseToVisibleConverter() : BoolConverter<Visibility>(Visibility.Collapsed, Visibility.Visible);
