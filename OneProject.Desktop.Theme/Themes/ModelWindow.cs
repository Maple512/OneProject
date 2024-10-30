namespace OneProject.Desktop.Theme.Themes;

public class ModelWindow : OPWindow
{
    public ModelWindow(Window owner)
    {
        Owner = Check.NotNull(owner);
        Icon = null;
        ResizeMode = ResizeMode.NoResize;
        ShowInTaskbar = false;
        IconVisibility = Visibility.Collapsed;
    }

    public static Window OpenModel(Window owner,
                                   string title,
                                   object content,
                                   object? data = null,
                                   double height = 400,
                                   double width = 600)
    {
        var window = new ModelWindow(owner)
        {
            Content = content,
            Title = title,
            DataContext = data,
            Width = width,
            Height = height,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        return window;
    }
}
