namespace OneProject.Desktop.Themes;

using OneProject.Desktop.Infrastructures;

public class ScaleHost : FrameworkElement
{
    public static readonly DependencyProperty ScaleProperty
     = PropertyHelper.Register<double, ScaleHost>(nameof(Scale));

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }
}
