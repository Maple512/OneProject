namespace OneProject.Desktop.Themes;

using System.Windows;
using System.Windows.Input;
using OneProject.Desktop.Components;
using OneProject.Desktop.Infrastructures;

[TemplatePart(Name = MinButton, Type = typeof(Button))]
[TemplatePart(Name = MaxButton, Type = typeof(Button))]
[TemplatePart(Name = RestoreButton, Type = typeof(Button))]
[TemplatePart(Name = CloseButton, Type = typeof(Button))]
[TemplatePart(Name = NotificationBox, Type = typeof(NotificationBox))]
public class OPWindow : Window
{
    private const string MinButton = "B_Min";
    private const string MaxButton = "B_Max";
    private const string RestoreButton = "B_Restore";
    private const string CloseButton = "B_Close";
    private const string NotificationBox = "P_NotificationBox";

    public static readonly DependencyProperty TitleMenuProperty =
        PropertyHelper.Register<object, OPWindow>(nameof(TitleMenu));

    public static readonly DependencyProperty IconVisibilityProperty
        = PropertyHelper.Register<Visibility, OPWindow>(nameof(IconVisibility), Visibility.Visible);

    public static readonly DependencyProperty CornerRadiusProperty
        = PropertyHelper.Register<CornerRadius, OPWindow>(nameof(CornerRadius), new CornerRadius(0));

    private Button? _closeButton;
    private Button? _maxButton;
    private Button? _minButton;
    private Button? _restoreButton;
    private NotificationBox? _notificationBox;

    static OPWindow() => DefaultStyleKeyProperty.OverrideMetadata<OPWindow>();

    public OPWindow()
    {
        // 修复WindowChrome导致的窗口大小错误
        var manual = SizeToContent.Manual;

        Loaded += (sender, args) =>
        {
            manual = SizeToContent;
        };

        ContentRendered += (sender, args) =>
        {
            SizeToContent = SizeToContent.Manual;
            Width = ActualWidth;
            Height = ActualHeight;
            SizeToContent = manual;
        };

        StateChanged += (sender, e) =>
        {
            if(ResizeMode is ResizeMode.CanMinimize or ResizeMode.NoResize)
            {
                if(WindowState is WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        };
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        Keyboard.ClearFocus();
    }

    public object TitleMenu
    {
        get => GetValue(TitleMenuProperty);
        set => SetValue(TitleMenuProperty, value);
    }

    public Visibility IconVisibility
    {
        get => (Visibility)GetValue(IconVisibilityProperty);
        set => SetValue(IconVisibilityProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _minButton = GetTemplateChild(MinButton) as Button;
        if(_minButton != null)
        {
            _minButton.Click += (sender, e) =>
            {
                WindowState = WindowState.Minimized;
            };
        }

        _maxButton = GetTemplateChild(MaxButton) as Button;
        if(_maxButton != null)
        {
            _maxButton.Click += (sender, e) =>
            {
                WindowState = WindowState.Maximized;
            };
        }

        _restoreButton = GetTemplateChild(RestoreButton) as Button;
        if(_restoreButton != null)
        {
            _restoreButton.Click += (sender, e) =>
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
                WindowState = WindowState.Normal;
            };
        }

        _closeButton = GetTemplateChild(CloseButton) as Button;
        if(_closeButton != null)
        {
            _closeButton.Click += (_, _) => Close();
        }

        _notificationBox = GetTemplateChild(NotificationBox) as NotificationBox;
        if(_notificationBox != null)
        {
            NotificationManager.Initialization(_notificationBox);
        }
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        WindowStartupLocation = WindowStartupLocation.Manual;

        if(WindowStyle is WindowStyle.None)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
        }
    }
}
