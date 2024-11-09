namespace OneProject.Desktop.Themes;

using System.Windows.Controls;
using OneProject.Desktop.Infrastructures;

public class OPToolTip : ToolTip
{
    static OPToolTip()
    {
        DefaultStyleKeyProperty.OverrideMetadata<OPToolTip>();
    }

    public bool AutoMove
    {
        get => (bool)GetValue(AutoMoveProperty);
        set => SetValue(AutoMoveProperty, value);
    }

    public static readonly DependencyProperty AutoMoveProperty
       = PropertyHelper.Register<bool, OPToolTip>(nameof(AutoMove),
           false,
           OnAutoMoveChanged);

    public double AutoMoveHorizontalOffset
    {
        get => (double)GetValue(AutoMoveHorizontalOffsetProperty);
        set => SetValue(AutoMoveHorizontalOffsetProperty, value);
    }

    public static readonly DependencyProperty AutoMoveHorizontalOffsetProperty
       = PropertyHelper.Register<double, OPToolTip>(nameof(AutoMoveHorizontalOffset), 16D);

    public double AutoMoveVerticalOffset
    {
        get => (double)GetValue(AutoMoveVerticalOffsetProperty);
        set => SetValue(AutoMoveVerticalOffsetProperty, value);
    }

    public static readonly DependencyProperty AutoMoveVerticalOffsetProperty
       = PropertyHelper.Register<double, OPToolTip>(nameof(AutoMoveVerticalOffset), 16D);

    private static void OnAutoMoveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        var toolTip = (OPToolTip)dependencyObject;
        if(eventArgs.OldValue != eventArgs.NewValue
            && eventArgs.NewValue is not null)
        {
            var autoMove = (bool)eventArgs.NewValue;
            if(autoMove)
            {
                toolTip.Opened += toolTip.ToolTip_Opened;
                toolTip.Closed += toolTip.ToolTip_Closed;
            }
            else
            {
                toolTip.Opened -= toolTip.ToolTip_Opened;
                toolTip.Closed -= toolTip.ToolTip_Closed;
            }
        }
    }

    private void ToolTip_Opened(object sender, RoutedEventArgs e)
    {
        var toolTip = (OPToolTip)sender;
        if(toolTip.PlacementTarget is FrameworkElement target)
        {
            // move the tooltip on opening to the correct position
            MoveToolTip(target, toolTip);

            target.MouseMove += ToolTipTargetPreviewMouseMove;
        }
    }

    private void ToolTip_Closed(object sender, RoutedEventArgs e)
    {
        var toolTip = (ToolTip)sender;
        if(toolTip.PlacementTarget is FrameworkElement target)
        {
            target.MouseMove -= ToolTipTargetPreviewMouseMove;
        }
    }

    private void ToolTipTargetPreviewMouseMove(object sender, MouseEventArgs e)
    {
        var toolTip = (sender is FrameworkElement target
            ? target.ToolTip
            : null) as OPToolTip;

        MoveToolTip(sender as IInputElement, toolTip);
    }

    private static void MoveToolTip(IInputElement? target, OPToolTip? toolTip)
    {
        if(toolTip is null
            || target is null
            || toolTip.PlacementTarget is null
            || PresentationSource.FromVisual(toolTip.PlacementTarget) is null)
        {
            return;
        }

        var hOffsetFromToolTip = toolTip.AutoMoveHorizontalOffset;
        var vOffsetFromToolTip = toolTip.AutoMoveVerticalOffset;

        MonitorHelper.MoveToolTip(target, toolTip, hOffsetFromToolTip, vOffsetFromToolTip);
    }
}
