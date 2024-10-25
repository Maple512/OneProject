namespace OneProject.Desktop.Theme.Themes;

using OneProject.Desktop.Theme.Infrastructures;

public class OPLoading : ContentControl
{
    static OPLoading() => DefaultStyleKeyProperty.OverrideMetadata<OPLoading>();

    public static readonly DependencyProperty IsLoadingProperty
    = PropertyHelper.Register<bool, OPLoading>(nameof(IsLoading));

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
}
