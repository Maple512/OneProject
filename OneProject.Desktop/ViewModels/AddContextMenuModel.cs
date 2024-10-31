namespace OneProject.Desktop.ViewModels;

using System.ComponentModel.DataAnnotations;
using OneProject.Desktop.Components;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Pages;

public partial class AddContextMenuModel : ModelBase<AddContextMenu>
{
    [Required]
    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string? icon;

    [ObservableProperty]
    private string? fileCommand;

    [ObservableProperty]
    private string directoryCommand;

    public AddContextMenuModel()
    {
        //title = "使用 VS Code 打开(&S)";
        //icon = @"E:\VSCode\Code.exe";
        //fileCommand = @"E:\VSCode\Code.exe %1";
        //directoryCommand = @"E:\VSCode\Code.exe %V";
    }

    [RelayCommand]
    private void Add()
    {
        if(string.IsNullOrWhiteSpace(Title))
        {
            NotificationManager.Warn("请输入名称");
            return;
        }
        try
        {
            if(!string.IsNullOrWhiteSpace(FileCommand))
            {
                WindowsRegistryManager.AddToFile(Title, Icon!, FileCommand);
            }

            if(!string.IsNullOrWhiteSpace(DirectoryCommand))
            {
                WindowsRegistryManager.AddToDirectory(Title, Icon!, DirectoryCommand);
            }

            NotificationManager.Success("已完成");
        }
        catch(Exception ex)
        {
            NotificationManager.Error(ex.Message);
        }
    }
}
