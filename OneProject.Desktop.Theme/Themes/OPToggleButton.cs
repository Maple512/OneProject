namespace OneProject.Desktop.Themes;

using OneProject.Desktop.Infrastructures;

public class OPToggleButton : ToggleButton
{
    static OPToggleButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPToggleButton>();
    }

    public static readonly DependencyProperty ABackgroundProperty
        = PropertyHelper.Register<Brush?, OPToggleButton>(nameof(ABackground));

    public Brush? ABackground
    {
        get => (Brush?)GetValue(ABackgroundProperty);
        set => SetValue(ABackgroundProperty, value);
    }

    public static readonly DependencyProperty AContentProperty
        = PropertyHelper.Register<object?, OPToggleButton>(nameof(AContent));

    public object? AContent
    {
        get => (object?)GetValue(AContentProperty);
        set => SetValue(AContentProperty, value);
    }

    public static readonly DependencyProperty AContentTemplateProperty
        = PropertyHelper.Register<DataTemplate?, OPToggleButton>(nameof(AContentTemplate));

    public DataTemplate? AContentTemplate
    {
        get => (DataTemplate?)GetValue(AContentTemplateProperty);
        set => SetValue(AContentTemplateProperty, value);
    }

    public static readonly DependencyProperty BBackgroundProperty
        = PropertyHelper.Register<Brush?, OPToggleButton>(nameof(BBackground));

    public Brush? BBackground
    {
        get => (Brush?)GetValue(BBackgroundProperty);
        set => SetValue(BBackgroundProperty, value);
    }

    public static readonly DependencyProperty BContentProperty
        = PropertyHelper.Register<object?, OPToggleButton>(nameof(BContent));

    public object? BContent
    {
        get => (object?)GetValue(BContentProperty);
        set => SetValue(BContentProperty, value);
    }

    public static readonly DependencyProperty BContentTemplateProperty
        = PropertyHelper.Register<DataTemplate?, OPToggleButton>(nameof(BContentTemplate));

    public DataTemplate? BContentTemplate
    {
        get => (DataTemplate?)GetValue(BContentTemplateProperty);
        set => SetValue(BContentTemplateProperty, value);
    }
}
