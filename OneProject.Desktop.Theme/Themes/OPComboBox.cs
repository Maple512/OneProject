namespace OneProject.Desktop.Themes;

using OneProject.Desktop.Infrastructures;

public class OPComboBox : ComboBox
{
    static OPComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPComboBox>();
    }

    public static readonly DependencyProperty CornerRadiusProperty
    = PropertyHelper.Register<CornerRadius, OPComboBox>(nameof(CornerRadius));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty MaxLengthProperty
    = PropertyHelper.Register<double, OPComboBox>(nameof(MaxLength));

    public double MaxLength
    {
        get => (double)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }
}
