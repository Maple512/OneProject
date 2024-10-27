namespace OneProject.Desktop.Pages;

using System;
using OneProject.Desktop.Theme.Componets;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();

        SendMessageBtn.Click += OnSendMessageBtnClick;
    }

    private void OnSendMessageBtnClick(object sender, RoutedEventArgs e)
    {
        var type = RandomHelper.GetRandomOf(NotificationType.Info, NotificationType.Success, NotificationType.Warn,
            NotificationType.Error);

        NotificationManager.AddNotification($"{DateTime.Now:mm:ss}: 随机消息", type);
    }
}
