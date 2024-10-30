namespace OneProject.Desktop.Pages;

using System;
using OneProject.Desktop.Componets;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();

        SendMessageBtn.Click += OnSendMessageBtnClick;
    }

    private void OnSendMessageBtnClick(object sender, RoutedEventArgs e)
    {
        var type = RandomHelper.GetRandomOf(NotificationTypeExtensions.GetValues());
        
        NotificationManager.AddNotification($"{DateTime.Now:mm:ss}: 随机消息", type);
    }
}
