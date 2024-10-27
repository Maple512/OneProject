namespace OneProject.Desktop.Componets;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

public partial class NotificationBox : UserControl
{
    private const int NotificationMax = 10;
    private readonly ObservableCollection<NotificationModel> _notifications = new(new(NotificationMax));
    private int _count;

    public NotificationBox()
    {
        InitializeComponent();

        DataContext = _notifications;
    }

    public void AddNotification(NotificationModel notification)
    {
        if(_notifications.Count == NotificationMax)
        {
            _notifications.RemoveAt(0);
        }

        notification.Id = _count++;

        _notifications.Add(notification);
    }

    public void RemoveAll() => _notifications.Clear();

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if(e.NewSize.Height != 0)
        {
            return;
        }

        var element = sender as FrameworkElement;

        var id = element!.Tag as int?;

        var notify = _notifications.FirstOrDefault(x => x.Id == id);
        if(notify != null)
        {
            _notifications.Remove(notify);
        }
    }
}

[FastEnum.Extensions]
public enum NotificationType
{
    Info,
    Success,
    Warn,
    Error,
}

public class NotificationModel(string content, NotificationType type)
{
    public int Id { get; set; }
    public string Content { get; } = content;
    public NotificationType Type { get; } = type;
}

public static class NotificationManager
{
    private static NotificationBox? _box;

    public static void Initialization(NotificationBox box)
    {
        _box = Check.NotNull(box);
    }

    public static void AddNotification(string? content, NotificationType type = NotificationType.Info)
    {
        if(content is null or { Length: 0, })
        {
            return;
        }

        var model = new NotificationModel(content, type);

        _box?.AddNotification(model);

        Log.Logger.Information($"Notify[{type.FastToString()}]: {content}");
    }

    public static void RemoveAllNotification()
    {
        _box?.RemoveAll();
    }
}

