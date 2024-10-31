namespace OneProject.Desktop.Assists;

using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

public static class ShadowAssist
{
    private static readonly DependencyPropertyKey ShadowKey = DependencyProperty.RegisterAttachedReadOnly(
        nameof(Shadow), typeof(Shadow), typeof(ShadowAssist), new(default(Shadow)));

    public static readonly DependencyProperty DarkenProperty
        = DependencyProperty.RegisterAttached("Darken", typeof(bool), typeof(ShadowAssist)
            , new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, DarkenChangedCallback));

    private static void SetShadow(DependencyObject element, Shadow? shadow) => element.SetValue(ShadowKey, shadow);

    private static Shadow? GetShadow(DependencyObject element)
        => (Shadow?)element.GetValue(ShadowKey.DependencyProperty);

    public static void SetDarken(DependencyObject element, bool value) => element.SetValue(DarkenProperty, value);

    public static bool GetDarken(DependencyObject element) => (bool)element.GetValue(DarkenProperty);

    private static void DarkenChangedCallback(DependencyObject element, DependencyPropertyChangedEventArgs args)
    {
        if(element is not UIElement ui)
        {
            return;
        }

        if(ui.Effect is not DropShadowEffect effect)
        {
            return;
        }

        double? toOpacity;

        if((bool)args.NewValue)
        {
            effect.BeginAnimation(DropShadowEffect.OpacityProperty, null);
            SetShadow(element, new(effect.Opacity));

            toOpacity = 1;
        }
        else
        {
            var shadow = GetShadow(element);
            if(shadow is null)
            {
                return;
            }

            toOpacity = shadow.Opacity;
        }

        var animation = new DoubleAnimation
        {
            To = toOpacity,
            Duration = new(new(0, 0, 0, 0, 180)),
            FillBehavior = FillBehavior.HoldEnd,
            EasingFunction = new CubicEase(),
            AccelerationRatio = 0.4,
            DecelerationRatio = 0.2,
        };

        effect.BeginAnimation(DropShadowEffect.OpacityProperty, animation);
    }

    private class Shadow(double opacity)
    {
        public double Opacity { get; } = opacity;
    }

    #region AttachedProperty : CacheModeProperty

    public static readonly DependencyProperty CacheModeProperty
        = DependencyProperty.RegisterAttached(
        nameof(CacheMode), typeof(CacheMode), typeof(ShadowAssist),
        new FrameworkPropertyMetadata(new BitmapCache { EnableClearType = true, SnapsToDevicePixels = true, },
            FrameworkPropertyMetadataOptions.Inherits));

    public static void SetCacheMode(DependencyObject element, CacheMode value)
        => element.SetValue(CacheModeProperty, value);

    public static CacheMode GetCacheMode(DependencyObject element) => (CacheMode)element.GetValue(CacheModeProperty);

    #endregion
}
