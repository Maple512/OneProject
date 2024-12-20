namespace OneProject.Desktop.Converters;

using Humanizer;

public class FileSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
    {
        long l => l.Bytes().ToString(),
        double d => d.Bytes().ToString(),
        _ => Binding.DoNothing,
    };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
