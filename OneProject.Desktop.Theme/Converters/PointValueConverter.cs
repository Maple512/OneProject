namespace OneProject.Desktop.Converters;

public class PointValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if(values?.Length == 2 && values[0] != null && values[1] != null)
        {
            if(double.TryParse(values[0].ToString(), out var x) &&
                double.TryParse(values[1].ToString(), out var y))
            {
                return new Point(x, y);
            }
        }

        return new Point();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        if(value is Point point)
        {
            return new object[] { point.X, point.Y };
        }

        return new object[0];
    }
}
