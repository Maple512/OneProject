namespace OneProject.Desktop.Pages;

using System.Windows.Controls;
using Microsoft.Win32;
using OneProject.Desktop.Themes;
using OneProject.Desktop.ViewModels;

public partial class AddContextMenu : UserControl
{
    public AddContextMenu()
    {
        DataContext = new AddContextMenuModel();

        InitializeComponent();
    }

    private void OnSelectFolderButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog()
        {
            Title = "选择执行程序",
            Filter = "应用程序(*.exe)|*.exe|所有文件(*.*)|*.*",
            DefaultExt = ".exe",
            Multiselect = false,
        };

        if(dialog.ShowDialog() == true)
        {
            var data = (AddContextMenuModel)DataContext;

            var name = Path.GetFileNameWithoutExtension(dialog.FileName);

            if(data.Title is null or { Length: 0 })
            {
                data.Title = name;
            }

            data.Icon = dialog.FileName;
            data.FileCommand = $"{dialog.FileName} %1";
            data.DirectoryCommand = $"{dialog.FileName} %V";
        }
    }

    private void OnAddButtonClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.BeginInvoke(() => Add((OPButton)sender, (AddContextMenuModel)DataContext));
    }

    private static void Add(OPButton button, AddContextMenuModel data)
    {
        button.IsLoading = true;

        try
        {
            data.AddCommand.Execute(null);
        }
        finally
        {
            button.IsLoading = false;
        }
    }
}
