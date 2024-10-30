namespace OneProject.Desktop.Theme.Themes;

using System.Windows.Controls;
using OneProject.Desktop.Theme.Infrastructures;

public enum OPTextBoxTypeEnum
{
    Default,
    UnderLine,
}

public class OPTextBox : TextBox
{
    private const double IconSizeDefault = 16D;

    static OPTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPTextBox>();
    }

    #region Common

    public OPTextBoxTypeEnum Type
    {
        get => (OPTextBoxTypeEnum)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly DependencyProperty TypeProperty
        = PropertyHelper.Register<OPTextBoxTypeEnum, OPTextBox>(nameof(Type), OPTextBoxTypeEnum.Default);

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty
        = PropertyHelper.Register<CornerRadius, OPTextBox>(nameof(CornerRadius), new CornerRadius(0));
    #endregion

    #region Hint
    public string? HintText
    {
        get => (string?)GetValue(HintTextProperty);
        set => SetValue(HintTextProperty, value);
    }

    public static readonly DependencyProperty HintTextProperty
        = PropertyHelper.Register<string?, OPTextBox>(nameof(HintText));

    public int IsShowCounter
    {
        get => (int)GetValue(IsShowCounterProperty);
        set => SetValue(IsShowCounterProperty, value);
    }

    public static readonly DependencyProperty IsShowCounterProperty
        = PropertyHelper.Register<int, OPTextBox>(nameof(IsShowCounter), 0);
    #endregion

    #region Content
    public object? PrefixContent
    {
        get => (object?)GetValue(PrefixContentProperty);
        set => SetValue(PrefixContentProperty, value);
    }

    public static readonly DependencyProperty PrefixContentProperty
        = PropertyHelper.Register<object?, OPTextBox>(nameof(PrefixContent));

    public object? SuffixContent
    {
        get => (object?)GetValue(SuffixContentProperty);
        set => SetValue(SuffixContentProperty, value);
    }

    public static readonly DependencyProperty SuffixContentProperty
        = PropertyHelper.Register<object?, OPTextBox>(nameof(SuffixContent));
    #endregion
}
