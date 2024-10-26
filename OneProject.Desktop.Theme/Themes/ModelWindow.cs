namespace OneProject.Desktop.Theme.Themes;

public class ModelWindow : OPWindow
{
    public ModelWindow(Window owner)
    {
        Owner = Check.NotNull(owner);
        Icon = null;
        ResizeMode = ResizeMode.NoResize;
        Width = 600;
        Height = 400;
        ShowInTaskbar = false;
        IconVisibility = Visibility.Collapsed;
    }

    public static Window OpenModel(Window owner, string title, object content, object? data = null)
    {
        var window = new ModelWindow(owner)
        {
            Content = content,
            Title = title,
            DataContext = data,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        window.ShowDialog();

        return window;
    }
}
