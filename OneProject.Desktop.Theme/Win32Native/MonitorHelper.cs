namespace OneProject.Desktop.Infrastructures;

using System.Windows;
using OneProject.Desktop.Win32Native;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

public static class MonitorHelper
{
    public static Rect GetOnScreenPosition(Rect rect, IntPtr windowHandle, bool ignoreTaskbar)
    {
        FindMaximumSingleMonitorRectangle(rect, out var screenSubRect, out var _);

        if(screenSubRect.Width.AreClose(0D) == false
            && screenSubRect.Height.AreClose(0D) == false)
        {
            return rect;
        }

        var monitor = MonitorFromRectOrWindow(rect.ToRECT(), windowHandle);
        if(monitor == IntPtr.Zero)
        {
            return rect;
        }

        var monitorInfo = PInvoke.GetMonitorInfo(monitor);

        var workAreaRect = ignoreTaskbar
            ? monitorInfo.rcMonitor
            : monitorInfo.rcWork;

        if(rect.Width > workAreaRect.GetWidth())
        {
            rect.Width = workAreaRect.GetWidth();
        }

        if(rect.Height > workAreaRect.GetHeight())
        {
            rect.Height = workAreaRect.GetHeight();
        }

        if(rect.Right > workAreaRect.right)
        {
            rect.X = workAreaRect.right - rect.Width;
        }

        if(rect.Left < workAreaRect.left)
        {
            rect.X = workAreaRect.left;
        }

        if(rect.Bottom > workAreaRect.bottom)
        {
            rect.Y = workAreaRect.bottom - rect.Height;
        }

        if(rect.Top < workAreaRect.top)
        {
            rect.Y = workAreaRect.top;
        }

        return rect;
    }

    private static void FindMaximumSingleMonitorRectangle(Rect windowRect, out Rect screenSubRect, out Rect monitorRect)
    {
        var windowRect2 = windowRect.ToRECT();

        FindMaximumSingleMonitorRectangle(windowRect2, out var screenSubRect2, out var monitorRect2);

        screenSubRect = new(screenSubRect2.GetPosition(), screenSubRect2.GetSize());

        monitorRect = new(monitorRect2.GetPosition(), monitorRect2.GetSize());
    }
    private static unsafe void FindMaximumSingleMonitorRectangle(RECT windowRect, out RECT screenSubRect, out RECT monitorRect)
    {
        var rect = new RECT
        {
            left = 0,
            right = 0,
            top = 0,
            bottom = 0
        };
        screenSubRect = rect;

        rect = new()
        {
            left = 0,
            right = 0,
            top = 0,
            bottom = 0
        };

        monitorRect = rect;

        var monitorFromRect = PInvoke.MonitorFromRect(&windowRect, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);
        if(monitorFromRect != IntPtr.Zero)
        {
            var monitorInfo = PInvoke.GetMonitorInfo(monitorFromRect);
            var lprcSrc = monitorInfo.rcWork;
            RECT lprcDst;
            PInvoke.IntersectRect(&lprcDst, &lprcSrc, &windowRect);
            screenSubRect = lprcDst;
            monitorRect = monitorInfo.rcWork;
        }
    }

    internal static IntPtr MonitorFromWindowPosOrWindow(WINDOWPOS windowpos, IntPtr hwnd)
    {
        var windowRect = windowpos.ToRECT();

        return MonitorFromRectOrWindow(windowRect, hwnd);
    }

    internal static unsafe IntPtr MonitorFromRectOrWindow(RECT windowRect, IntPtr hwnd)
    {
        var monitorFromWindow = PInvoke.MonitorFromWindow(new(hwnd), MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);

        if(windowRect.IsEmpty())
        {
            return monitorFromWindow;
        }

        var monitorFromRect = PInvoke.MonitorFromRect(&windowRect, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);

        return monitorFromRect;
    }

    internal static MONITORINFO MonitorInfoFromWindow(IntPtr hWnd)
    {
        var hMonitor = PInvoke.MonitorFromWindow(new(hWnd), MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);

        var monitorInfo = PInvoke.GetMonitorInfo(hMonitor);

        return monitorInfo;
    }

    public static void MoveToolTip(IInputElement? target, ToolTip? toolTip, double hOffsetFromToolTip, double vOffsetFromToolTip)
    {
        if(toolTip is null
            || target is null
            || toolTip.PlacementTarget is null
            || PresentationSource.FromVisual(toolTip.PlacementTarget) is null)
        {
            return;
        }

        toolTip.SetCurrentValue(ToolTip.PlacementProperty, PlacementMode.Relative);

        var dpi = DpiHelper.GetDpi(toolTip);

        Debug.WriteLine(">>dpi       >> x: {0} \t y: {1}", dpi.DpiScaleX, dpi.DpiScaleY);

        var hDPIOffset = DpiHelper.TransformToDeviceX(toolTip.PlacementTarget, hOffsetFromToolTip, dpi.DpiScaleX);
        var vDPIOffset = DpiHelper.TransformToDeviceY(toolTip.PlacementTarget, vOffsetFromToolTip, dpi.DpiScaleY);

        var position = Mouse.GetPosition(toolTip.PlacementTarget);
        var newHorizontalOffset = position.X + hDPIOffset;
        var newVerticalOffset = position.Y + vDPIOffset;

        var topLeftFromScreen = toolTip.PlacementTarget.PointToScreen(new Point(0, 0));
        if(TryGetMonitorInfoFromPoint(out var mInfo) == false)
        {
            return;
        }

        Debug.WriteLine(">>rcWork    >> w: {0} \t h: {1}", mInfo.rcWork.GetWidth(), mInfo.rcWork.GetHeight());
        Debug.WriteLine(">>rcMonitor >> w: {0} \t h: {1}", mInfo.rcMonitor.GetWidth(), mInfo.rcMonitor.GetHeight());

        var monitorWorkWidth = Math.Abs(mInfo.rcWork.GetWidth());
        var monitorWorkHeight = Math.Abs(mInfo.rcWork.GetHeight());

        if(monitorWorkWidth == 0
            || monitorWorkHeight == 0)
        {
            Trace.TraceError("Got wrong monitor info values ({0})", mInfo.rcWork);
            return;
        }

        topLeftFromScreen.X = -mInfo.rcWork.left + topLeftFromScreen.X;
        topLeftFromScreen.Y = -mInfo.rcWork.top + topLeftFromScreen.Y;

        var locationX = (int)topLeftFromScreen.X % monitorWorkWidth;
        var locationY = (int)topLeftFromScreen.Y % monitorWorkHeight;

        var renderDpiWidth = DpiHelper.TransformToDeviceX(toolTip.PlacementTarget, toolTip.RenderSize.Width, dpi.DpiScaleX);
        var rightX = locationX + newHorizontalOffset + renderDpiWidth;
        if(rightX > monitorWorkWidth)
        {
            newHorizontalOffset = position.X - toolTip.RenderSize.Width - (0.5 * hDPIOffset);
        }

        var renderDPIHeight = DpiHelper.TransformToDeviceY(toolTip.PlacementTarget, toolTip.RenderSize.Height, dpi.DpiScaleY);
        var bottomY = locationY + newVerticalOffset + renderDPIHeight;
        if(bottomY > monitorWorkHeight)
        {
            newVerticalOffset = position.Y - toolTip.RenderSize.Height - (0.5 * vDPIOffset);
        }

        Debug.WriteLine(">>tooltip   >> bY: {0:F} \t rX: {1:F}", bottomY, rightX);

        toolTip.HorizontalOffset = newHorizontalOffset;
        toolTip.VerticalOffset = newVerticalOffset;

        Debug.WriteLine(">>offset    >> ho: {0:F} \t vo: {1:F}", toolTip.HorizontalOffset, toolTip.VerticalOffset);
    }

    /// <summary>
    /// Gets the monitor information from the current cursor position.
    /// </summary>
    /// <returns>True when getting the monitor information was successful.</returns>
    internal static bool TryGetMonitorInfoFromPoint(out MONITORINFO monitorInfo)
    {
        try
        {
            var cursorPos = PInvoke.GetCursorPos(out var point);

            var monitor = PInvoke.MonitorFromPoint(point, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);

            if(monitor != IntPtr.Zero)
            {
                monitorInfo = PInvoke.GetMonitorInfo(monitor);
                return true;
            }
        }
        catch(UnauthorizedAccessException ex)
        {
            Trace.TraceError("Could not get the monitor info with the current cursor position: {0}", ex);
        }

        monitorInfo = default;
        return false;
    }
}
