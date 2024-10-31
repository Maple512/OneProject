namespace OneProject.Desktop.Themes;

using System.ComponentModel;
using System.Windows.Controls;
using OneProject.Desktop.Infrastructures;

internal interface IStackMeasure
{
    bool IsScrolling { get; }
    UIElementCollection InternalChildren { get; }
    Orientation Orientation { get; }
    bool CanVerticallyScroll { get; }
    bool CanHorizontallyScroll { get; }
    void OnScrollChange();
}

/// <summary>
///     Internal interface for scrolling information of elements which
///     need stack like measure.
/// </summary>
internal interface IStackMeasureScrollData
{
    Vector Offset { get; set; }
    Size Viewport { get; set; }
    Size Extent { get; set; }
    Vector ComputedOffset { get; set; }
    void SetPhysicalViewport(double value);
}

public class OPStackPanel : Panel, IScrollInfo, IStackMeasure
{
    private const double _scrollLineDelta = 16.0;

    //------------------------------------------------------
    //
    //  Private Fields
    //
    //------------------------------------------------------

    #region Private Fields

    // Logical scrolling and virtualization data.
    private ScrollData _scrollData;

    #endregion Private Fields

    //-------------------------------------------------------------------
    //
    //  Constructors
    //
    //-------------------------------------------------------------------

    #region Constructors

    static OPStackPanel() => DefaultStyleKeyProperty.OverrideMetadata<OPStackPanel>();

    public OPStackPanel()
    {
        _scrollData = new ScrollData();

    }

    #endregion Constructors

    #region Public Methods

    //-----------------------------------------------------------
    //  IScrollInfo Methods
    //-----------------------------------------------------------

    #region IScrollInfo Methods

    /// <summary>
    ///     Scroll content by one line to the top.
    /// </summary>
    public void LineUp()
        => SetVerticalOffset(VerticalOffset - (Orientation == Orientation.Vertical ? 1.0 : _scrollLineDelta));

    /// <summary>
    ///     Scroll content by one line to the bottom.
    /// </summary>
    public void LineDown()
        => SetVerticalOffset(VerticalOffset + (Orientation == Orientation.Vertical ? 1.0 : _scrollLineDelta));

    /// <summary>
    ///     Scroll content by one line to the left.
    /// </summary>
    public void LineLeft()
        => SetHorizontalOffset(HorizontalOffset - (Orientation == Orientation.Horizontal ? 1.0 : _scrollLineDelta));

    /// <summary>
    ///     Scroll content by one line to the right.
    /// </summary>
    public void LineRight()
        => SetHorizontalOffset(HorizontalOffset + (Orientation == Orientation.Horizontal ? 1.0 : _scrollLineDelta));

    /// <summary>
    ///     Scroll content by one page to the top.
    /// </summary>
    public void PageUp() => SetVerticalOffset(VerticalOffset - ViewportHeight);

    /// <summary>
    ///     Scroll content by one page to the bottom.
    /// </summary>
    public void PageDown() => SetVerticalOffset(VerticalOffset + ViewportHeight);

    /// <summary>
    ///     Scroll content by one page to the left.
    /// </summary>
    public void PageLeft() => SetHorizontalOffset(HorizontalOffset - ViewportWidth);

    /// <summary>
    ///     Scroll content by one page to the right.
    /// </summary>
    public void PageRight() => SetHorizontalOffset(HorizontalOffset + ViewportWidth);

    /// <summary>
    ///     Scroll content by one page to the top.
    /// </summary>
    public void MouseWheelUp()
    {
        if(CanMouseWheelVerticallyScroll)
        {
            SetVerticalOffset(VerticalOffset - (SystemParameters.WheelScrollLines *
                                                (Orientation == Orientation.Vertical ? 1.0 : _scrollLineDelta)));
        }
        else
        {
            PageUp();
        }
    }

    /// <summary>
    ///     Scroll content by one page to the bottom.
    /// </summary>
    public void MouseWheelDown()
    {
        if(CanMouseWheelVerticallyScroll)
        {
            SetVerticalOffset(VerticalOffset + (SystemParameters.WheelScrollLines *
                                                (Orientation == Orientation.Vertical ? 1.0 : _scrollLineDelta)));
        }
        else
        {
            PageDown();
        }
    }

    /// <summary>
    ///     Scroll content by one page to the left.
    /// </summary>
    public void MouseWheelLeft()
        => SetHorizontalOffset(HorizontalOffset -
                               (3.0 * (Orientation == Orientation.Horizontal ? 1.0 : _scrollLineDelta)));

    /// <summary>
    ///     Scroll content by one page to the right.
    /// </summary>
    public void MouseWheelRight()
        => SetHorizontalOffset(HorizontalOffset +
                               (3.0 * (Orientation == Orientation.Horizontal ? 1.0 : _scrollLineDelta)));

    /// <summary>
    ///     Set the HorizontalOffset to the passed value.
    /// </summary>
    public void SetHorizontalOffset(double offset)
    {
        EnsureScrollData();
        var scrollX = ValidateInputOffset(offset, "HorizontalOffset");
        if(!DoubleUtil.AreClose(scrollX, _scrollData._offset.X))
        {
            _scrollData._offset.X = scrollX;
            InvalidateMeasure();
        }
    }

    /// <summary>
    ///     Set the VerticalOffset to the passed value.
    /// </summary>
    public void SetVerticalOffset(double offset)
    {
        EnsureScrollData();
        var scrollY = ValidateInputOffset(offset, "VerticalOffset");
        if(!DoubleUtil.AreClose(scrollY, _scrollData._offset.Y))
        {
            _scrollData._offset.Y = scrollY;
            InvalidateMeasure();
        }
    }

    /// <summary>
    ///     OPStackPanel implementation of <seealso cref="IScrollInfo.MakeVisible" />.
    /// </summary>
    // The goal is to change offsets to bring the child into view, and return a rectangle in our space to make visible.
    // The rectangle we return is in the physical dimension the input target rect transformed into our pace.
    // In the logical dimension, it is our immediate child's rect.
    // Note: This code presently assumes we/children are layout clean.  See work item 22269 for more detail.
    public Rect MakeVisible(Visual visual, Rect rectangle)
    {
        var newOffset = new Vector();
        var newRect = new Rect();

        // We can only work on visuals that are us or children.
        // An empty rect has no size or position.  We can't meaningfully use it.
        if(rectangle.IsEmpty
           || visual == null
           || visual == this
           || !IsAncestorOf(visual))
        {
            return Rect.Empty;
        }
#pragma warning disable 56506
        // Compute the child's rect relative to (0,0) in our coordinate space.
        // This is a false positive by PreSharp. visual cannot be null because of the 'if' check above
        var childTransform = visual.TransformToAncestor(this);
#pragma warning restore 56506
        rectangle = childTransform.TransformBounds(rectangle);

        // We can't do any work unless we're scrolling.
        if(!IsScrolling)
        {
            return rectangle;
        }

        // Bring the target rect into view in the physical dimension.
        MakeVisiblePhysicalHelper(rectangle, ref newOffset, ref newRect);

        // Bring our child containing the visual into view.
        var childIndex = FindChildIndexThatParentsVisual(visual);
        MakeVisibleLogicalHelper(childIndex, ref newOffset, ref newRect);

        // We have computed the scrolling offsets; validate and scroll to them.
        newOffset.X = CoerceOffset(newOffset.X, _scrollData._extent.Width, _scrollData._viewport.Width);
        newOffset.Y = CoerceOffset(newOffset.Y, _scrollData._extent.Height, _scrollData._viewport.Height);

        if(!DoubleUtil.AreClose(newOffset, _scrollData._offset))
        {
            _scrollData._offset = newOffset;
            InvalidateMeasure();
            OnScrollChange();
        }

        // Return the rectangle
        return newRect;
    }

    #endregion

    #endregion

    //-------------------------------------------------------------------
    //
    //  Public Properties
    //
    //-------------------------------------------------------------------

    #region Public Properties

    /// <summary>
    ///     Specifies dimension of children stacking.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    ///     DependencyProperty for <see cref="Orientation" /> property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty
        = PropertyHelper.Register<Orientation, OPStackPanel>(nameof(Orientation), Orientation.Vertical,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
    OnSizeChanged, IsValidOrientation);

    public static readonly DependencyProperty SpaceProperty
        = PropertyHelper.Register<double, OPStackPanel>(nameof(Space), 5, OnSizeChanged);

    public double Space
    {
        get => (double)GetValue(SpaceProperty);
        set => SetValue(SpaceProperty, value);
    }

    /// <summary>
    ///     This property is always true because this panel has vertical or horizontal orientation
    /// </summary>
    protected override bool HasLogicalOrientation => true;

    /// <summary>
    ///     Orientation of the panel if its layout is in one dimension.
    ///     Otherwise HasLogicalOrientation is false and LogicalOrientation should be ignored
    /// </summary>
    protected override Orientation LogicalOrientation => Orientation;

    //-----------------------------------------------------------
    //  IScrollInfo Properties
    //-----------------------------------------------------------

    #region IScrollInfo Properties

    /// <summary>
    ///     OPStackPanel reacts to this property by changing it's child measurement algorithm.
    ///     If scrolling in a dimension, infinite space is allowed the child; otherwise, available size is preserved.
    /// </summary>
    [DefaultValue(false)]
    public bool CanHorizontallyScroll
    {
        get
        {
            if(_scrollData == null)
            {
                return false;
            }

            return _scrollData._allowHorizontal;
        }
        set
        {
            EnsureScrollData();
            if(_scrollData._allowHorizontal != value)
            {
                _scrollData._allowHorizontal = value;
                InvalidateMeasure();
            }
        }
    }

    /// <summary>
    ///     OPStackPanel reacts to this property by changing it's child measurement algorithm.
    ///     If scrolling in a dimension, infinite space is allowed the child; otherwise, available size is preserved.
    /// </summary>
    [DefaultValue(false)]
    public bool CanVerticallyScroll
    {
        get
        {
            if(_scrollData == null)
            {
                return false;
            }

            return _scrollData._allowVertical;
        }
        set
        {
            EnsureScrollData();
            if(_scrollData._allowVertical != value)
            {
                _scrollData._allowVertical = value;
                InvalidateMeasure();
            }
        }
    }

    /// <summary>
    ///     ExtentWidth contains the horizontal size of the scrolled content element in 1/96"
    /// </summary>
    public double ExtentWidth
    {
        get
        {
            if(_scrollData == null)
            {
                return 0.0;
            }

            return _scrollData._extent.Width;
        }
    }

    /// <summary>
    ///     ExtentHeight contains the vertical size of the scrolled content element in 1/96"
    /// </summary>
    public double ExtentHeight
    {
        get
        {
            if(_scrollData == null)
            {
                return 0.0;
            }

            return _scrollData._extent.Height;
        }
    }

    /// <summary>
    ///     ViewportWidth contains the horizontal size of content's visible range in 1/96"
    /// </summary>
    public double ViewportWidth
    {
        get
        {
            if(_scrollData == null)
            {
                return 0.0;
            }

            return _scrollData._viewport.Width;
        }
    }

    /// <summary>
    ///     ViewportHeight contains the vertical size of content's visible range in 1/96"
    /// </summary>
    public double ViewportHeight
    {
        get
        {
            if(_scrollData == null)
            {
                return 0.0;
            }

            return _scrollData._viewport.Height;
        }
    }

    /// <summary>
    ///     HorizontalOffset is the horizontal offset of the scrolled content in 1/96".
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double HorizontalOffset
    {
        get
        {
            if(_scrollData == null)
            {
                return 0.0;
            }

            return _scrollData._computedOffset.X;
        }
    }

    /// <summary>
    ///     VerticalOffset is the vertical offset of the scrolled content in 1/96".
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double VerticalOffset
    {
        get
        {
            if(_scrollData == null)
            {
                return 0.0;
            }

            return _scrollData._computedOffset.Y;
        }
    }

    /// <summary>
    ///     ScrollOwner is the container that controls any scrollbars, headers, etc... that are dependant
    ///     on this IScrollInfo's properties.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ScrollViewer ScrollOwner
    {
        get
        {
            EnsureScrollData();

            return _scrollData._scrollOwner;
        }
        set
        {
            EnsureScrollData();
            if(value != _scrollData._scrollOwner)
            {
                ResetScrolling(this);
                _scrollData._scrollOwner = value;
            }
        }
    }

    #endregion IScrollInfo Properties

    #endregion Public Properties

    //-------------------------------------------------------------------
    //
    //  Protected Methods
    //
    //-------------------------------------------------------------------

    #region Protected Methods

    /// <summary>
    ///     General OPStackPanel layout behavior is to grow unbounded in the "stacking" direction (Size To Content).
    ///     Children in this dimension are encouraged to be as large as they like.  In the other dimension,
    ///     OPStackPanel will assume the maximum size of its children.
    /// </summary>
    /// <remarks>
    ///     When scrolling, OPStackPanel will not grow in layout size but effectively add the children on a z-plane which
    ///     will probably be clipped by some parent (typically a ScrollContentPresenter) to Stack's size.
    /// </remarks>
    /// <param name="constraint" >Constraint</param>
    /// <returns>Desired size</returns>
    protected override Size MeasureOverride(Size constraint)
    {
        var stackDesiredSize = new Size();
        var children = InternalChildren;
        var layoutSlotSize = constraint;
        var fHorizontal = Orientation == Orientation.Horizontal;
        int firstViewport; // First child index in the viewport.
        var lastViewport
            = -1; // Last child index in the viewport.  -1 indicates we have not yet iterated through the last child.

        double logicalVisibleSpace, childLogicalSize;

        //
        // Initialize child sizing and iterator data
        // Allow children as much size as they want along the stack.
        //
        if(fHorizontal)
        {
            layoutSlotSize.Width = double.PositiveInfinity;
            if(IsScrolling && CanVerticallyScroll)
            {
                layoutSlotSize.Height = double.PositiveInfinity;
            }

            firstViewport = IsScrolling ? CoerceOffsetToInteger(_scrollData.Offset.X, children.Count) : 0;
            logicalVisibleSpace = constraint.Width;
        }
        else
        {
            layoutSlotSize.Height = double.PositiveInfinity;
            if(IsScrolling && CanHorizontallyScroll)
            {
                layoutSlotSize.Width = double.PositiveInfinity;
            }

            firstViewport = IsScrolling ? CoerceOffsetToInteger(_scrollData.Offset.Y, children.Count) : 0;
            logicalVisibleSpace = constraint.Height;
        }

        //
        //  Iterate through children.
        //  While we still supported virtualization, this was hidden in a child iterator (see source history).
        //
        var spaceCount = 0;
        for(int i = 0, count = children.Count; i < count; ++i)
        {
            // Get next child.
            var child = children[i];

            if(child is null or { Visibility: Visibility.Collapsed, })
            {
                continue;
            }

            // Measure the child.
            child.Measure(layoutSlotSize);
            var childDesiredSize = child.DesiredSize;

            // Accumulate child size.
            if(fHorizontal)
            {
                stackDesiredSize.Width += childDesiredSize.Width;
                stackDesiredSize.Height = Math.Max(stackDesiredSize.Height, childDesiredSize.Height);
                childLogicalSize = childDesiredSize.Width;
            }
            else
            {
                stackDesiredSize.Width = Math.Max(stackDesiredSize.Width, childDesiredSize.Width);
                stackDesiredSize.Height += childDesiredSize.Height;
                childLogicalSize = childDesiredSize.Height;
            }

            if(IsInvalidElement(child as FrameworkElement))
            {
                spaceCount++;
            }

            // Adjust remaining viewport space if we are scrolling and within the viewport region.
            // While scrolling (not virtualizing), we always measure children before and after the viewport.
            if(IsScrolling && lastViewport == -1 && i >= firstViewport)
            {
                logicalVisibleSpace -= childLogicalSize;
                if(DoubleUtil.LessThanOrClose(logicalVisibleSpace, 0.0))
                {
                    lastViewport = i;
                }
            }
        }

        if(spaceCount > 1)
        {
            var space = (spaceCount - 1) * Space;
            if(fHorizontal)
            {
                stackDesiredSize.Width += space;
            }
            else
            {
                stackDesiredSize.Height += space;
            }
        }

        //
        // Compute Scrolling stuff.
        //
        if(IsScrolling)
        {
            // Compute viewport and extent.
            var viewport = constraint;
            var extent = stackDesiredSize;
            var offset = _scrollData.Offset;

            // If we have not yet set the last child in the viewport, set it to the last child.
            if(lastViewport == -1)
            {
                lastViewport = children.Count - 1;
            }

            // If we or children have resized, it's possible that we can now display more content.
            // This is true if we started at a nonzero offeset and still have space remaining.
            // In this case, we loop back through previous children until we run out of space.
            while(firstViewport > 0)
            {
                var projectedLogicalVisibleSpace = logicalVisibleSpace;
                if(fHorizontal)
                {
                    projectedLogicalVisibleSpace -= children[firstViewport - 1].DesiredSize.Width;
                }
                else
                {
                    projectedLogicalVisibleSpace -= children[firstViewport - 1].DesiredSize.Height;
                }

                // If we have run out of room, break.
                if(DoubleUtil.LessThan(projectedLogicalVisibleSpace, 0.0))
                {
                    break;
                }

                // Adjust viewport
                firstViewport--;
                logicalVisibleSpace = projectedLogicalVisibleSpace;
            }

            var logicalExtent = children.Count;
            var logicalViewport = lastViewport - firstViewport;

            // We are conservative when estimating a viewport, not including the last element in case it is only partially visible.
            // We want to count it if it is fully visible (>= 0 space remaining) or the only element in the viewport.
            if(logicalViewport == 0 || DoubleUtil.GreaterThanOrClose(logicalVisibleSpace, 0.0))
            {
                logicalViewport++;
            }

            if(fHorizontal)
            {
                _scrollData.SetPhysicalViewport(viewport.Width);
                viewport.Width = logicalViewport;
                extent.Width = logicalExtent;
                offset.X = firstViewport;
                offset.Y = Math.Max(0, Math.Min(offset.Y, extent.Height - viewport.Height));
            }
            else
            {
                _scrollData.SetPhysicalViewport(viewport.Height);
                viewport.Height = logicalViewport;
                extent.Height = logicalExtent;
                offset.Y = firstViewport;
                offset.X = Math.Max(0, Math.Min(offset.X, extent.Width - viewport.Width));
            }

            // Since we can offset and clip our content, we never need to be larger than the parent suggestion.
            // If we returned the full size of the content, we would always be so big we didn't need to scroll.  :)
            stackDesiredSize.Width = Math.Min(stackDesiredSize.Width, constraint.Width);
            stackDesiredSize.Height = Math.Min(stackDesiredSize.Height, constraint.Height);

            // Verify Scroll Info, invalidate ScrollOwner if necessary.
            VerifyScrollingData(this, _scrollData, viewport, extent, offset);
        }

        return stackDesiredSize;
    }

    private static bool IsInvalidElement(FrameworkElement? element)
        => element is not null && element.DesiredSize.Width > 0 && element.DesiredSize.Height > 0;

    /// <summary>
    ///     Content arrangement.
    /// </summary>
    /// <param name="arrangeSize" >Arrange size</param>
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        var children = InternalChildren;
        var fHorizontal = Orientation == Orientation.Horizontal;
        var rcChild = new Rect(arrangeSize);
        var previousChildSize = 0.0;

        //
        // Compute scroll offset and seed it into rcChild.
        //
        if(IsScrolling)
        {
            if(fHorizontal)
            {
                rcChild.X = ComputePhysicalFromLogicalOffset(this, _scrollData.ComputedOffset.X, true, Space);
                rcChild.Y = -1.0 * _scrollData.ComputedOffset.Y;
            }
            else
            {
                rcChild.X = -1.0 * _scrollData.ComputedOffset.X;
                rcChild.Y = ComputePhysicalFromLogicalOffset(this, _scrollData.ComputedOffset.Y, false, Space);
            }
        }

        //
        // Arrange and Position Children.
        //
        var startSpace = false;
        for(int i = 0, count = children.Count; i < count; ++i)
        {
            var child = children[i];

            if(child is null or { Visibility: Visibility.Collapsed, })
            {
                continue;
            }

            if(fHorizontal)
            {
                rcChild.X += previousChildSize;
                previousChildSize = child.DesiredSize.Width;
                rcChild.Width = previousChildSize;
                rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);
            }
            else
            {
                rcChild.Y += previousChildSize;
                previousChildSize = child.DesiredSize.Height;
                rcChild.Height = previousChildSize;
                rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);
            }

            var isInvalid = IsInvalidElement(child as FrameworkElement);
            if(startSpace && isInvalid)
            {
                if(fHorizontal)
                {
                    rcChild.X += Space;
                }
                else
                {
                    rcChild.Y += Space;
                }
            }

            child.Arrange(rcChild);

            if(isInvalid && !startSpace)
            {
                startSpace = true;
            }
        }

        return arrangeSize;
    }

    #endregion Protected Methods

    //------------------------------------------------------
    //
    //  Private Methods
    //
    //------------------------------------------------------

    #region Private Methods

    private void EnsureScrollData() => _scrollData ??= new();

    private static void ResetScrolling(OPStackPanel element)
    {
        element.InvalidateMeasure();

        // Clear scrolling data.  Because of thrash (being disconnected & reconnected, &c...), we may
        if(element.IsScrolling)
        {
            element._scrollData.ClearLayout();
        }
    }

    // OnScrollChange is an override called whenever the IScrollInfo exposed scrolling state changes on this element.
    // At the time this method is called, scrolling state is in its new, valid state.
    private void OnScrollChange() => ScrollOwner?.InvalidateScrollInfo();

    private static void VerifyScrollingData(
        IStackMeasure measureElement,
        IStackMeasureScrollData scrollData,
        Size viewport,
        Size extent,
        Vector offset)
    {
        var fValid = true;

        Debug.Assert(measureElement.IsScrolling);

        fValid &= DoubleUtil.AreClose(viewport, scrollData.Viewport);
        fValid &= DoubleUtil.AreClose(extent, scrollData.Extent);
        fValid &= DoubleUtil.AreClose(offset, scrollData.ComputedOffset);
        scrollData.Offset = offset;

        if(!fValid)
        {
            scrollData.Viewport = viewport;
            scrollData.Extent = extent;
            scrollData.ComputedOffset = offset;
            measureElement.OnScrollChange();
        }
    }

    // Translates a logical (child index) offset to a physical (1/96") when scrolling.
    // If virtualizing, it makes the assumption that the logicalOffset is always the first in the visual collection
    //   and thus returns 0.
    // If not virtualizing, it assumes that children are Measure clean; should only be called after running Measure.
    private static double ComputePhysicalFromLogicalOffset(
        IStackMeasure arrangeElement,
        double logicalOffset,
        bool fHorizontal,
        double space)
    {
        var physicalOffset = 0.0;

        var children = arrangeElement.InternalChildren;
        Debug.Assert(logicalOffset == 0 || (logicalOffset > 0 && logicalOffset < children.Count));

        for(var i = 0; i < logicalOffset; i++)
        {
            physicalOffset -= fHorizontal
                ? children[i].DesiredSize.Width
                : children[i].DesiredSize.Height;
        }

        return physicalOffset;
    }

    private int FindChildIndexThatParentsVisual(Visual child)
    {
        DependencyObject dependencyObjectChild = child;

        var parent = VisualTreeHelper.GetParent(child);

        while(parent != this)
        {
            dependencyObjectChild = parent;
            parent = VisualTreeHelper.GetParent(dependencyObjectChild);
            if(parent == null)
            {
                throw new ArgumentException("Panel must be the parent or ancestor of Visual.", nameof(child));
            }
        }

        var children = Children;
        //The Downcast is ok because OPStackPanel's
        //child has to be a UIElement to be in this.Children collection
        return children.IndexOf((UIElement)dependencyObjectChild);
    }

    private void MakeVisiblePhysicalHelper(Rect r, ref Vector newOffset, ref Rect newRect)
    {
        double viewportOffset;
        double viewportSize;
        double targetRectOffset;
        double targetRectSize;
        double minPhysicalOffset;

        var fHorizontal = Orientation == Orientation.Horizontal;
        if(fHorizontal)
        {
            viewportOffset = _scrollData._computedOffset.Y;
            viewportSize = ViewportHeight;
            targetRectOffset = r.Y;
            targetRectSize = r.Height;
        }
        else
        {
            viewportOffset = _scrollData._computedOffset.X;
            viewportSize = ViewportWidth;
            targetRectOffset = r.X;
            targetRectSize = r.Width;
        }

        targetRectOffset += viewportOffset;

        minPhysicalOffset = ComputeScrollOffsetWithMinimalScroll(
            viewportOffset, viewportOffset + viewportSize, targetRectOffset, targetRectOffset + targetRectSize);

        // Compute the visible rectangle of the child relative to the viewport.
        var left = Math.Max(targetRectOffset, minPhysicalOffset);
        targetRectSize = Math.Max(Math.Min(targetRectSize + targetRectOffset, minPhysicalOffset + viewportSize) - left,
            0);
        targetRectOffset = left;
        targetRectOffset -= viewportOffset;

        if(fHorizontal)
        {
            newOffset.Y = minPhysicalOffset;
            newRect.Y = targetRectOffset;
            newRect.Height = targetRectSize;
        }
        else
        {
            newOffset.X = minPhysicalOffset;
            newRect.X = targetRectOffset;
            newRect.Width = targetRectSize;
        }
    }

    private void MakeVisibleLogicalHelper(int childIndex, ref Vector newOffset, ref Rect newRect)
    {
        var fHorizontal = Orientation == Orientation.Horizontal;
        int firstChildInView;
        int newFirstChild;
        int viewportSize;
        double childOffsetWithinViewport = 0;

        if(fHorizontal)
        {
            firstChildInView = (int)_scrollData._computedOffset.X;
            viewportSize = (int)_scrollData._viewport.Width;
        }
        else
        {
            firstChildInView = (int)_scrollData._computedOffset.Y;
            viewportSize = (int)_scrollData._viewport.Height;
        }

        newFirstChild = firstChildInView;

        // If the target child is before the current viewport, move the viewport to put the child at the top.
        if(childIndex < firstChildInView)
        {
            newFirstChild = childIndex;
        }
        // If the target child is after the current viewport, move the viewport to put the child at the bottom.
        else if(childIndex > firstChildInView + viewportSize - 1)
        {
            var childDesiredSize = InternalChildren[childIndex].DesiredSize;
            var nextChildSize = fHorizontal ? childDesiredSize.Width : childDesiredSize.Height;
            var viewportSpace = _scrollData._physicalViewport - nextChildSize;
            var i = childIndex;
            while(i > 0 && DoubleUtil.GreaterThanOrClose(viewportSpace, 0.0))
            {
                i--;
                childDesiredSize = InternalChildren[i].DesiredSize;
                nextChildSize = fHorizontal ? childDesiredSize.Width : childDesiredSize.Height;
                childOffsetWithinViewport += nextChildSize;
                viewportSpace -= nextChildSize;
            }

            if(i != childIndex && DoubleUtil.LessThan(viewportSpace, 0.0))
            {
                childOffsetWithinViewport -= nextChildSize;
                i++;
            }

            newFirstChild = i;
        }

        if(fHorizontal)
        {
            newOffset.X = newFirstChild;
            newRect.X = childOffsetWithinViewport;
            newRect.Width = InternalChildren[childIndex].DesiredSize.Width;
        }
        else
        {
            newOffset.Y = newFirstChild;
            newRect.Y = childOffsetWithinViewport;
            newRect.Height = InternalChildren[childIndex].DesiredSize.Height;
        }
    }

    private static int CoerceOffsetToInteger(double offset, int numberOfItems)
    {
        int iNewOffset;

        if(double.IsNegativeInfinity(offset))
        {
            iNewOffset = 0;
        }
        else if(double.IsPositiveInfinity(offset))
        {
            iNewOffset = numberOfItems - 1;
        }
        else
        {
            iNewOffset = (int)offset;
            iNewOffset = Math.Max(Math.Min(numberOfItems - 1, iNewOffset), 0);
        }

        return iNewOffset;
    }

    //-----------------------------------------------------------
    // Avalon Property Callbacks/Overrides
    //-----------------------------------------------------------

    #region Avalon Property Callbacks/Overrides

    /// <summary>
    ///     <see cref="PropertyMetadata.PropertyChangedCallback" />
    /// </summary>
    private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = d as OPStackPanel;
        // Since Orientation is so essential to logical scrolling/virtualization, we synchronously check if
        // the new value is different and clear all scrolling data if so.
        ResetScrolling(panel!);
    }

    #endregion

    #endregion Private Methods

    //------------------------------------------------------
    //
    //  Private Properties
    //
    //------------------------------------------------------

    #region Private Properties

    private bool IsScrolling => _scrollData != null && _scrollData._scrollOwner != null;

    //
    //  This property
    //  1. Finds the correct initial size for the _effectiveValues store on the current DependencyObject
    //  2. This is a performance optimization
    //
    internal int EffectiveValuesInitialSize => 9;

    bool IStackMeasure.IsScrolling => IsScrolling;

    UIElementCollection IStackMeasure.InternalChildren => InternalChildren;

    void IStackMeasure.OnScrollChange() => OnScrollChange();

    private bool CanMouseWheelVerticallyScroll => SystemParameters.WheelScrollLines > 0;

    #endregion Private Properties

    //------------------------------------------------------
    //
    //  Private Structures / Classes
    //
    //------------------------------------------------------

    #region Private Structures Classes

    //-----------------------------------------------------------
    // ScrollData class
    //-----------------------------------------------------------

    #region ScrollData

    // Helper class to hold scrolling data.
    // This class exists to reduce working set when OPStackPanel is used outside a scrolling situation.
    // Standard "extra pointer always for less data sometimes" cache savings model:
    //      !Scroll [1xReference]
    //      Scroll  [1xReference] + [6xDouble + 1xReference]
    private class ScrollData : IStackMeasureScrollData
    {
        // For Stack/Flow, the two dimensions of properties are in different units:
        // 1. The "logically scrolling" dimension uses items as units.
        // 2. The other dimension physically scrolls.  Units are in Avalon pixels (1/96").
        internal bool _allowHorizontal;
        internal bool _allowVertical;
        internal Vector _computedOffset = new(0, 0);
        internal Size _extent; // Extent is the total number of children (logical dimension) or physical size
        internal Vector _offset; // Scroll offset of content.  Positive corresponds to a visually upward offset.
        internal double _physicalViewport; // The physical size of the viewport for the items dimension above.
        internal ScrollViewer? _scrollOwner; // ScrollViewer to which we're attached.
        internal Size _viewport; // ViewportSize is in {pixels x items} (or vice-versa).

        public Vector Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public Size Viewport
        {
            get => _viewport;
            set => _viewport = value;
        }

        public Size Extent
        {
            get => _extent;
            set => _extent = value;
        }

        public Vector ComputedOffset
        {
            get => _computedOffset;
            set => _computedOffset = value;
        }

        public void SetPhysicalViewport(double value) => _physicalViewport = value;

        // Clears layout generated data.
        // Does not clear scrollOwner, because unless resetting due to a scrollOwner change, we won't get reattached.
        internal void ClearLayout()
        {
            _offset = new();
            _viewport = _extent = new();
            _physicalViewport = 0;
        }
    }

    internal static bool IsValidOrientation(object o)
    {
        var value = (Orientation)o;
        return value is Orientation.Horizontal
            or Orientation.Vertical;
    }

    internal static double ComputeScrollOffsetWithMinimalScroll(
        double topView,
        double bottomView,
        double topChild,
        double bottomChild)
    {
        var alignTop = false;
        var alignBottom = false;
        return ComputeScrollOffsetWithMinimalScroll(topView, bottomView, topChild, bottomChild, ref alignTop,
            ref alignBottom);
    }

    internal static double ComputeScrollOffsetWithMinimalScroll(
        double topView,
        double bottomView,
        double topChild,
        double bottomChild,
        ref bool alignTop,
        ref bool alignBottom)
    {
        // # CHILD POSITION       CHILD SIZE      SCROLL      REMEDY
        // 1 Above viewport       <= viewport     Down        Align top edge of child & viewport
        // 2 Above viewport       > viewport      Down        Align bottom edge of child & viewport
        // 3 Below viewport       <= viewport     Up          Align bottom edge of child & viewport
        // 4 Below viewport       > viewport      Up          Align top edge of child & viewport
        // 5 Entirely within viewport             NA          No scroll.
        // 6 Spanning viewport                    NA          No scroll.
        //
        // Note: "Above viewport" = childTop above viewportTop, childBottom above viewportBottom
        //       "Below viewport" = childTop below viewportTop, childBottom below viewportBottom
        // These child thus may overlap with the viewport, but will scroll the same direction/

        var fAbove = DoubleUtil.LessThan(topChild, topView) && DoubleUtil.LessThan(bottomChild, bottomView);
        var fBelow = DoubleUtil.GreaterThan(bottomChild, bottomView) && DoubleUtil.GreaterThan(topChild, topView);
        var fLarger = bottomChild - topChild > bottomView - topView;

        // Handle Cases:  1 & 4 above
        if((fAbove && !fLarger)
           || (fBelow && fLarger)
           || alignTop)
        {
            alignTop = true;
            return topChild;
        }

        // Handle Cases: 2 & 3 above

        if(fAbove || fBelow || alignBottom)
        {
            alignBottom = true;
            return bottomChild - (bottomView - topView);
        }

        // Handle cases: 5 & 6 above.
        return topView;
    }

    internal static double CoerceOffset(double offset, double extent, double viewport)
    {
        if(offset > extent - viewport)
        {
            offset = extent - viewport;
        }

        if(offset < 0)
        {
            offset = 0;
        }

        return offset;
    }

    internal static double ValidateInputOffset(double offset, string parameterName)
    {
        if(double.IsNaN(offset))
        {
            throw new ArgumentOutOfRangeException(parameterName, $"'{parameterName}' parameter value cannot be NaN.");
        }

        return Math.Max(0.0, offset);
    }

    #endregion ScrollData

    #endregion Private Structures Classes
}
