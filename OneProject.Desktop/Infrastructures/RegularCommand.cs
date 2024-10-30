namespace OneProject.Desktop.Infrastructures;

using CliWrap;
using OneProject.Desktop.Componets;

/// <summary>
///     通用命令
/// </summary>
public static class RegularCommand
{
    /// <summary>
    ///     打开资源管理器，并选中给定的文件，如果有
    /// </summary>
    public static ICommand OpenExplorer
        => new AsyncRelayCommand<string>(static async path =>
        {
            if(path is null or { Length: 0, })
            {
                return;
            }

            try
            {
                await Cli.Wrap("explorer")
                    .WithWorkingDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
                    .WithArguments(["/select,", path,])
                    .ExecuteAsync();
            }
            catch(Exception) { }
        });

    /// <summary>
    ///     使用默认程序打开文件
    /// </summary>
    public static ICommand OpenFile
        => new AsyncRelayCommand<string>(static async path =>
        {
            if(path is null or { Length: 0, })
            {
                return;
            }

            try
            {
                await Cli.Wrap("explorer")
                    .WithWorkingDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
                    .WithArguments(path)
                    .ExecuteAsync();
            }
            catch(Exception) { }
        });

    /// <summary>
    ///     复制对象到系统剪贴板上
    /// </summary>
    /// g
    public static ICommand CopyObject
        => new RelayCommand<object>(static text =>
        {
            Clipboard.SetDataObject(text);
        });

    /// <summary>
    ///     在浏览器中打开网址
    /// </summary>
    public static ICommand OpenInBrowser
        => new AsyncRelayCommand<string>(static async url =>
        {
            if(Uri.TryCreate(url, UriKind.Absolute, out _) == false)
            {
                NotificationManager.AddNotification($"Not a valid URL: {url}", NotificationType.Warn);

                return;
            }

            await Cli.Wrap("cmd")
                .WithWorkingDirectory(Environment.GetFolderPath(Environment.SpecialFolder.System))
                .WithArguments("/c", "start", url!)
                .ExecuteAsync();
        });
}
