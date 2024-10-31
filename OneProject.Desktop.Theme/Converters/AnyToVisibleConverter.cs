namespace OneProject.Desktop.Converters;

public class AnyToVisibleConverter : IMultiValueConverter
{
    public object? Value { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.Any(x => x == Value)
            ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class AllToVisibleConverter : IMultiValueConverter
{
    public object? Value { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.All(x => x == Value)
            ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
