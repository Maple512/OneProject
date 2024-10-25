namespace OneProject.Desktop.Theme.Converters;

public class NullConverter<TTo> : IValueConverter
    where TTo : notnull
{
    private readonly TTo _notNull;
    private readonly TTo _null;

    protected NullConverter(TTo @null, TTo notNull)
    {
        _null = @null;
        _notNull = notNull;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value switch
        {
            null => _null,
            string str => str.Length > 0 ? _notNull : _null,
            ICollection collection => collection.Count > 0 ? _notNull : _null,
            IEnumerable enumerable => EnumerableHelper.Any(enumerable) ? _notNull : _null,
            _ => _notNull,
        };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

public class NotNullToVisibleConverter() : NullConverter<Visibility>(Visibility.Collapsed, Visibility.Visible);

public class NullToVisibleConverter() : NullConverter<Visibility>(Visibility.Visible, Visibility.Collapsed);

public class NotNullToTrueConverter() : NullConverter<bool>(false, true);

public class NullToTrueConverter() : NullConverter<bool>(true, false);
