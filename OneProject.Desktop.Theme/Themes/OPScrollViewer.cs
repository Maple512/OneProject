namespace OneProject.Desktop.Themes;

using System.Windows.Interop;
using OneProject.Desktop.Infrastructures;

public class OPScrollViewer : ScrollViewer
{
    static OPScrollViewer()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPScrollViewer>();

        //HorizontalOffsetProperty.OverrideMetadata(typeof(OPScrollViewer), new FrameworkPropertyMetadata(0D, OnSyncHorizontalOffsetChanged));
    }

    public new static readonly DependencyProperty HorizontalOffsetProperty
        = PropertyHelper.Register<double, OPScrollViewer>(nameof(HorizontalOffset), 0D, OnSyncHorizontalOffsetChanged);

    public new double HorizontalOffset
    {
        get => (double)GetValue(HorizontalOffsetProperty);
        set => SetValue(HorizontalOffsetProperty, value);
    }

    public static readonly DependencyProperty IsAutoHideEnabledProperty
        = PropertyHelper.Register<bool, OPScrollViewer>(nameof(IsAutoHideEnabled));

    public bool IsAutoHideEnabled
    {
        get => (bool)GetValue(IsAutoHideEnabledProperty);
        set => SetValue(IsAutoHideEnabledProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty
    = PropertyHelper.Register<CornerRadius, OPScrollViewer>(nameof(CornerRadius));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty ShowSeparatorsProperty
    = PropertyHelper.Register<bool, OPScrollViewer>(nameof(ShowSeparators));

    public bool ShowSeparators
    {
        get => (bool)GetValue(ShowSeparatorsProperty);
        set => SetValue(ShowSeparatorsProperty, value);
    }

    public static readonly DependencyProperty IgnorePaddingProperty
    = PropertyHelper.Register<bool, OPScrollViewer>(nameof(IgnorePadding));

    public bool IgnorePadding
    {
        get => (bool)GetValue(IgnorePaddingProperty);
        set => SetValue(IgnorePaddingProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollHookProperty
    = PropertyHelper.Register<HwndSourceHook, OPScrollViewer>(nameof(HorizontalScrollHook));

    public HwndSourceHook HorizontalScrollHook
    {
        get => (HwndSourceHook)GetValue(HorizontalScrollHookProperty);
        set => SetValue(HorizontalScrollHookProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollSourceProperty
    = PropertyHelper.Register<HwndSource, OPScrollViewer>(nameof(HorizontalScrollSource));

    public HwndSource HorizontalScrollSource
    {
        get => (HwndSource)GetValue(HorizontalScrollSourceProperty);
        set => SetValue(HorizontalScrollSourceProperty, value);
    }

    public static readonly DependencyProperty SupportHorizontalScrollProperty
    = PropertyHelper.Register<bool, OPScrollViewer>(nameof(SupportHorizontalScroll), OnSupportHorizontalScrollChanged);

    public bool SupportHorizontalScroll
    {
        get => (bool)GetValue(SupportHorizontalScrollProperty);
        set => SetValue(SupportHorizontalScrollProperty, value);
    }

    public static readonly DependencyProperty BubbleVerticalScrollProperty
    = PropertyHelper.Register<bool, OPScrollViewer>(nameof(BubbleVerticalScroll), OnBubbleVerticalScrollChanged);

    public bool BubbleVerticalScroll
    {
        get => (bool)GetValue(BubbleVerticalScrollProperty);
        set => SetValue(BubbleVerticalScrollProperty, value);
    }

    private static void OnSyncHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var scrollViewer = d as ScrollViewer;

        scrollViewer?.ScrollToHorizontalOffset((double)e.NewValue);
    }

    private static void OnSupportHorizontalScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //Based on: https://blog.walterlv.com/post/handle-horizontal-scrolling-of-touchpad-en.html
        if(d is OPScrollViewer scrollViewer)
        {
            if(scrollViewer.IsLoaded)
            {
                DoOnLoaded(scrollViewer);
            }
            else
            {
                WeakEventManager<OPScrollViewer, RoutedEventArgs>.AddHandler(scrollViewer, nameof(Loaded),
                    OnLoaded);
                WeakEventManager<OPScrollViewer, RoutedEventArgs>.AddHandler(scrollViewer, nameof(Unloaded),
                    OnUnloaded);
            }
        }

        static void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if(sender is OPScrollViewer sv)
            {
                DoOnLoaded(sv);
            }
        }

        static void DoOnLoaded(OPScrollViewer sv)
        {
            if(sv.SupportHorizontalScroll)
            {
                RegisterHook(sv);
            }
            else
            {
                RemoveHook(sv);
            }
        }

        static void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            if(sender is OPScrollViewer sv)
            {
                RemoveHook(sv);
            }
        }

        static void RemoveHook(OPScrollViewer scrollViewer)
        {
            if(scrollViewer.GetValue(HorizontalScrollHookProperty) is HwndSourceHook hook &&
               scrollViewer.GetValue(HorizontalScrollSourceProperty) is HwndSource source)
            {
                source.RemoveHook(hook);
                scrollViewer.SetValue(HorizontalScrollHookProperty, null);
            }
        }

        static void RegisterHook(OPScrollViewer scrollViewer)
        {
            RemoveHook(scrollViewer);
            if(PresentationSource.FromVisual(scrollViewer) is HwndSource source)
            {
                HwndSourceHook hook = Hook;
                scrollViewer.SetValue(HorizontalScrollSourceProperty, source);
                scrollViewer.SetValue(HorizontalScrollHookProperty, hook);
                source.AddHook(hook);
            }

            IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                const int WM_MOUSEHWHEEL = 0x020E;
                switch(msg)
                {
                    case WM_MOUSEHWHEEL:
                        int tilt = (short)((wParam.ToInt64() >> 16) & 0xFFFF);
                        OnMouseTilt(tilt);
                        return 1;
                }

                return IntPtr.Zero;
            }

            void OnMouseTilt(int tilt)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + tilt);
            }
        }
    }

    private static void OnBubbleVerticalScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(d is OPScrollViewer scrollViewer)
        {
            if(scrollViewer.IsLoaded)
            {
                DoOnLoaded(scrollViewer);
            }
            else
            {
                WeakEventManager<OPScrollViewer, RoutedEventArgs>.AddHandler(scrollViewer, nameof(Loaded),
                    OnLoaded);
                WeakEventManager<OPScrollViewer, RoutedEventArgs>.AddHandler(scrollViewer, nameof(Unloaded),
                    OnUnloaded);
            }
        }

        static void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if(sender is OPScrollViewer sv)
            {
                DoOnLoaded(sv);
            }
        }

        static void DoOnLoaded(OPScrollViewer sv)
        {
            if(sv.BubbleVerticalScroll)
            {
                RegisterHook(sv);
            }
            else
            {
                RemoveHook(sv);
            }
        }

        static void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            if(sender is OPScrollViewer sv)
            {
                RemoveHook(sv);
            }
        }

        static void RemoveHook(OPScrollViewer scrollViewer)
        {
            scrollViewer.RemoveHandler(UIElement.MouseWheelEvent, (RoutedEventHandler)ScrollViewerOnMouseWheel);
        }

        static void RegisterHook(OPScrollViewer scrollViewer)
        {
            RemoveHook(scrollViewer);
            scrollViewer.AddHandler(UIElement.MouseWheelEvent, (RoutedEventHandler)ScrollViewerOnMouseWheel, true);
        }

        // This relay is only needed because the UIElement.AddHandler() has strict requirements for the signature of the passed Delegate
        static void ScrollViewerOnMouseWheel(object sender, RoutedEventArgs e)
        {
            HandleMouseWheel(sender, (MouseWheelEventArgs)e);
        }

        static void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (OPScrollViewer)sender;

            if(scrollViewer.GetVisualAncestry().Skip(1).FirstOrDefault() is not UIElement parentUiElement)
            {
                return;
            }

            // Re-raise the mouse wheel event on the visual parent to bubble it upwards
            e.Handled = true;
            var mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender,
            };
            parentUiElement.RaiseEvent(mouseWheelEventArgs);
        }
    }
}
