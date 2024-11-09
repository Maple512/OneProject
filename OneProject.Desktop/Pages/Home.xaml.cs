namespace OneProject.Desktop.Pages;

using System;
using OneProject.Desktop.Assists;
using OneProject.Desktop.Components;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();
    }

    private void OnSendMessageBtnClick(object sender, RoutedEventArgs e)
    {
        var type = RandomHelper.GetRandomOf(NotificationTypeExtensions.GetValues());

        NotificationManager.Notify($"{DateTime.Now:mm:ss}: 随机消息", type);
    }

    private async void ShowLoadingAnimation(object sender, RoutedEventArgs e)
    {
        LoadingAssist.SetIsLoading(C_Buttons, true);

        await Task.Delay(2 * 1000);

        LoadingAssist.SetIsLoading(C_Buttons, false);
    }
}
