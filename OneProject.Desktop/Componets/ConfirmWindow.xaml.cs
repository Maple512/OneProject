namespace OneProject.Desktop.Components;

using System.Windows.Controls;
using System.Windows.Threading;
using OneProject.Desktop.Themes;
using OneProject.Desktop.ViewModels;

public partial class ConfirmWindow : UserControl
{
    public ConfirmWindow()
    {
        InitializeComponent();
    }

    public static void Open(ConfirmModel model)
    {
        Dispatcher.CurrentDispatcher.BeginInvoke(static (ConfirmModel data) => OpenCore(data), model);
    }

    private static void OpenCore(ConfirmModel model)
    {
        var confirm = new ConfirmWindow();

        var window = ModelWindow.OpenModel(App.Current.MainWindow, model.Title, confirm, height: 200, width: 300);

        var okCommand = model.OkCommand;

        model.CancelCommand = new RelayCommand(window.Close);
        model.OkCommand = new RelayCommand<object?>((parameter) =>
        {
            okCommand?.Execute(parameter);

            window.Close();
        });

        confirm.DataContext = model;

        window.ShowDialog();
    }
}
