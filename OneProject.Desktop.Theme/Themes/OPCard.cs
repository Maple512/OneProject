namespace OneProject.Desktop.Theme.Themes;

using System;
using OneProject.Desktop.Theme.Infrastructures;

[TemplatePart(Name = ClipBorderPartName, Type = typeof(Border))]
public class OPCard : ContentControl
{
    public const string ClipBorderPartName = "PART_ClipBorder";

    public static readonly DependencyProperty TitleProperty
        = PropertyHelper.Register<object, OPCard>(nameof(Title));

    public static readonly DependencyProperty HeaderProperty
        = PropertyHelper.Register<object, OPCard>(nameof(Header));

    public static readonly DependencyProperty TitleActionProperty
        = PropertyHelper.Register<object, OPCard>(nameof(TitleAction));

    public static readonly DependencyProperty HasHeaderProperty
        = PropertyHelper.Register<Visibility, OPCard>(nameof(HasHeader),
            new FrameworkPropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty CornerRadiusProperty
        = PropertyHelper.Register<CornerRadius, OPCard>(nameof(CornerRadius), new(0),
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            CornerRadiusChangedCallback);

    public static readonly DependencyProperty ClipContentProperty =
        DependencyProperty.Register("ClipContent", typeof(bool), typeof(OPCard), new(false));

    private Border? _clipBorder;

    static OPCard()
        => DefaultStyleKeyProperty.OverrideMetadata(typeof(OPCard), new FrameworkPropertyMetadata(typeof(OPCard)));

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public object TitleAction
    {
        get => GetValue(TitleActionProperty);
        set => SetValue(TitleActionProperty, value);
    }

    public Visibility HasHeader
    {
        get => (Visibility)GetValue(HasHeaderProperty);
        set => SetValue(HasHeaderProperty, value);
    }

    public bool ClipContent
    {
        get => (bool)GetValue(ClipContentProperty);
        set => SetValue(ClipContentProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _clipBorder = Template.FindName(ClipBorderPartName, this) as Border;

        HasHeader = Header is null && Title is null && TitleAction is null
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        UpdateContentClip();
    }

    private void UpdateContentClip()
    {
        if(_clipBorder is null)
        {
            return;
        }

        var farPointX = Math.Max(0, _clipBorder.ActualWidth);
        var farPointY = Math.Max(0, _clipBorder.ActualHeight);
        var farPoint = new Point(farPointX, farPointY);

        var clipRect = new Rect(new(0, 0), farPoint);

        ContentClip = new RectangleGeometry(clipRect, CornerRadius.BottomLeft, CornerRadius.TopRight);
    }

    #region CornerRadius

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private static void CornerRadiusChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (OPCard)d;

        card.UpdateContentClip();
    }

    #endregion

    #region DependencyProperty : ContentClipProperty

    private static readonly DependencyPropertyKey ContentClipPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(ContentClip), typeof(Geometry), typeof(OPCard),
            new(default(Geometry)));

    public Geometry? ContentClip
    {
        get => (Geometry?)GetValue(ContentClipProperty);
        private set => SetValue(ContentClipPropertyKey, value);
    }

    public static readonly DependencyProperty ContentClipProperty
        = ContentClipPropertyKey.DependencyProperty;

    #endregion
}
