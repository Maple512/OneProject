namespace OneProject.Desktop.Theme.Converters;

using System.Globalization;

public enum ComparisionType
{
    Equal,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
}

public class ComparableConverter : IValueConverter
{
    public ComparableConverter(Visibility left, Visibility right, ComparisionType type)
    {
        Left = left;
        Right = right;
        Type = type;
    }

    public ComparisionType Type { get; set; }

    public Visibility Right { get; set; }

    public Visibility Left { get; set; }

    public object RightValue { get; set; } = 0;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(Type is ComparisionType.Equal)
        {
            return value == RightValue ? Left : Right;
        }

        if(value is not IComparable c)
        {
            return Binding.DoNothing;
        }

        var result = c.CompareTo(RightValue);

        return Type switch
        {
            ComparisionType.GreaterThan => result > 0 ? Left : Right,
            ComparisionType.GreaterThanOrEqual => result >= 0 ? Left : Right,
            ComparisionType.LessThan => result < 0 ? Left : Right,
            ComparisionType.LessThanOrEqual => result <= 0 ? Left : Right,
            _ => Visibility.Collapsed,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class EqualToVisibleConverter : ComparableConverter
{
    public EqualToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.Equal)
    {
    }
}
public class NotEqualToVisibleConverter : ComparableConverter
{
    public NotEqualToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.Equal)
    {
    }
}

public class GreaterThanToVisibleConverter : ComparableConverter
{
    public GreaterThanToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.GreaterThan)
    {
    }
}
public class NotGreaterThanToVisibleConverter : ComparableConverter
{
    public NotGreaterThanToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.GreaterThan)
    {
    }
}

public class GreaterThanOrEqualToVisibleConverter : ComparableConverter
{
    public GreaterThanOrEqualToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.GreaterThanOrEqual)
    {
    }
}
public class NotGreaterThanOrEqualToVisibleConverter : ComparableConverter
{
    public NotGreaterThanOrEqualToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.GreaterThanOrEqual)
    {
    }
}

public class LessThanToVisibleConverter : ComparableConverter
{
    public LessThanToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.LessThan)
    {
    }
}
public class NotLessThanToVisibleConverter : ComparableConverter
{
    public NotLessThanToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.LessThan)
    {
    }
}

public class LessThanOrEqualToVisibleConverter : ComparableConverter
{
    public LessThanOrEqualToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.LessThanOrEqual)
    {
    }
}
public class NotLessThanOrEqualToVisibleConverter : ComparableConverter
{
    public NotLessThanOrEqualToVisibleConverter() : base(Visibility.Visible, Visibility.Collapsed, ComparisionType.LessThanOrEqual)
    {
    }
}
