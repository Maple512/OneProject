namespace OneProject.Desktop.Theme.Themes;

using System.Windows.Controls;
using OneProject.Desktop.Theme.Infrastructures;

public enum OPButtonType
{
    Default,
    Primary,
    Text,
}

public class OPButton : Button
{
    static OPButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPButton>();
    }

    #region Properties

    public static readonly DependencyProperty TypeProperty
        = PropertyHelper.Register<OPButtonType, OPButton>(nameof(Type));

    public OPButtonType Type
    {
        get => (OPButtonType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly DependencyProperty IsLoadingProperty
        = PropertyHelper.Register<bool, OPButton>(nameof(IsLoading));

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty
        = PropertyHelper.Register<CornerRadius, OPButton>(nameof(CornerRadius));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty IconProperty =
        PropertyHelper.Register<IconKind?, OPButton >(nameof(Icon));

    public IconKind? Icon
    {
        get => (IconKind?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty IconSizeProperty =
        PropertyHelper.Register<double?, OPButton>(nameof(IconSize));

    public double? IconSize
    {
        get => (double?)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    #endregion
}
