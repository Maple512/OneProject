namespace OneProject.Desktop.ViewModels;

public class MenuItemModel
{
    private object? _content;
    private MenuItemModel(string name, Type contentType, object? data = null)
    {
        Title = name;
        ContentType = contentType;
        Data = data;
    }
    public Type ContentType { get; }

    public string Title { get; }

    public object? Data { get; }

    public object? Content => _content ??= CreateContent();

    public static MenuItemModel Create<T>(string name, object? dataContent = null)
    => new(name, typeof(T), dataContent);

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
