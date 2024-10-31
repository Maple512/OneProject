namespace OneProject.Desktop.Assists;

public static class LoadingAssist
{
    public static readonly DependencyProperty IsLoadingProperty =
        DependencyProperty.RegisterAttached(
            "IsLoading", typeof(bool), typeof(LoadingAssist), new FrameworkPropertyMetadata(false));

    public static void SetIsLoading(DependencyObject element, bool value) => element.SetValue(IsLoadingProperty, value);

    public static bool GetIsLoading(DependencyObject element) => (bool)element.GetValue(IsLoadingProperty);
}
