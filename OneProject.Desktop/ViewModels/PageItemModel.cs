namespace OneProject.Desktop.ViewModels;

using OneProject.Desktop.Themes;

public class PageItemModel
{
    private object? _content;
    private PageItemModel(string name, Type contentType, IconKind? icon = null, object? data = null)
    {
        Title = name;
        ContentType = contentType;
        Data = data;
        Icon = icon;
    }

    public Type ContentType { get; }

    public string Title { get; }

    public IconKind? Icon { get; }

    public object? Data { get; }

    public ICollection<PageItemModel>? SubItems { get; }

    public object? Content => _content ??= CreateContent();

    public static PageItemModel Create<T>(string name, IconKind? icon = null, object? dataContent = null)
    => new(name, typeof(T), icon, dataContent);

    private object? CreateContent()
    {
        var instance = Activator.CreateInstance(ContentType);

        if(Data is not null
           && Content is FrameworkElement element)
        {
            element.DataContext = Data;
        }

        return instance;
    }
}
