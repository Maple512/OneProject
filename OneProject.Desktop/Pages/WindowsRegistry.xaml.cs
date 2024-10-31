namespace OneProject.Desktop.Pages;

using System.Windows.Controls;
using CliWrap;
using OneProject.Desktop.Assists;
using OneProject.Desktop.ViewModels;

public partial class WindowsRegistry : UserControl
{
    public WindowsRegistry()
    {
        InitializeComponent();

        DataContext = new WindowsRegistryModel();
    }

    private void OnShowRegEdit(object sender, RoutedEventArgs e)
    {
        Dispatcher.BeginInvoke(async () =>
        {
            try
            {
                await Cli.Wrap("regedit")
                    .ExecuteAsync();
            }
            catch(Exception) { }
        });
    }

    private void OnBackupButtonClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.BeginInvoke(() => Backup());
    }

    async Task Backup()
    {
        LoadingAssist.SetIsLoading(P_Card, true);

        try
        {
            var model = (DataContext as WindowsRegistryModel)!;

            await model.BackupCommand.ExecuteAsync(null);
        }
        finally
        {
            LoadingAssist.SetIsLoading(P_Card, false);
        }
    }
}
