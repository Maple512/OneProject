namespace OneProject.Desktop.Themes;

using OneProject.Desktop.Infrastructures;

public class OPThumb : Thumb
{
    static OPThumb()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPThumb>();

        HeightProperty.OverrideMetadata(typeof(OPThumb), new FrameworkPropertyMetadata(double.NaN));
        WidthProperty.OverrideMetadata(typeof(OPThumb), new FrameworkPropertyMetadata(double.NaN));
    }

    public static readonly DependencyProperty CornerRadiusProperty
        = PropertyHelper.Register<CornerRadius, OPThumb>(nameof(CornerRadius));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty VerticalWidthProperty
        = PropertyHelper.Register<double, OPThumb>(nameof(VerticalWidth));

    public double VerticalWidth
    {
        get => (double)GetValue(VerticalWidthProperty);
        set => SetValue(VerticalWidthProperty, value);
    }

    public static readonly DependencyProperty HorizontalHeightProperty
        = PropertyHelper.Register<double, OPThumb>(nameof(HorizontalHeight));

    public double HorizontalHeight
    {
        get => (double)GetValue(HorizontalHeightProperty);
        set => SetValue(HorizontalHeightProperty, value);
    }
}
