namespace OneProject.Desktop.Infrastructures;

using System.IO;
using System.Threading.Tasks;
using CliWrap;
using FastEnum;

[Extensions]
public enum RegistryType
{
    HKEY_CLASSES_ROOT,
    HKEY_CURRENT_USER,
    HKEY_LOCAL_MACHINE,
    HKEY_USERS,
    HKEY_CURRENT_CONFIG,
}

public class RegistryManager
{
    /// <summary>
    /// 将指定文件导入注册表
    /// </summary>
    /// <param name="reg"></param>
    /// <param name="key"></param>
    public static async Task ImportAsync(FileInfo reg)
    {
        FileHelper.ThrowIfExisted(reg);

        await Cli.Wrap("reg")
             .WithArguments("import", reg.FullName)
             .ExecuteAsync();
    }

    /// <summary>
    /// 将指定键以及子项全部导入指定的文件中
    /// </summary>
    /// <param name="key"></param>
    /// <param name="reg"></param>
    public static async Task ExportAsync(string key, FileInfo reg)
    {
        Check.NotNullOrWhiteSpace(key);

        FileHelper.ThrowIfExisted(reg);

        await Cli.Wrap("regedit")
              .WithArguments("/E", reg.FullName, key)
              .ExecuteAsync();

        while(true)
        {
            await Task.Delay(200);

            if(new FileInfo(reg.FullName).Length > 0)
            {
                break;
            }
        }
    }

    public static async Task ExportHKCRAsync(FileInfo reg)
    {
        await ExportAsync(RegistryType.HKEY_CLASSES_ROOT.FastToString(), reg);
    }

    public static async Task ExportHKCUAsync(FileInfo reg)
    {
        await ExportAsync(RegistryType.HKEY_CURRENT_USER.FastToString(), reg);
    }

    public static async Task ExportHKLMAsync(FileInfo reg)
    {
        await ExportAsync(RegistryType.HKEY_LOCAL_MACHINE.FastToString(), reg);
    }

    public static async Task ExportHKUAsync(FileInfo reg)
    {
        await ExportAsync(RegistryType.HKEY_USERS.FastToString(), reg);
    }

    public static async Task ExportHKCCAsync(FileInfo reg)
    {
        await ExportAsync(RegistryType.HKEY_CURRENT_CONFIG.FastToString(), reg);
    }

    /// <summary>
    /// 导出所有注册表项
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static async Task ExportAllAsync(FileInfo target)
    {
        var hkcr = FileHelper.TryGetFileInfo(Path.Combine(target.DirectoryName!, "hkcr.reg"));
        await ExportAsync(RegistryType.HKEY_CLASSES_ROOT.FastToString(), hkcr);
        await RemoveFileFirstLineAsync(hkcr);

        var hkcu = FileHelper.TryGetFileInfo(Path.Combine(target.DirectoryName!, "hkcu.reg"));
        await ExportAsync(RegistryType.HKEY_CURRENT_USER.FastToString(), hkcu);
        await RemoveFileFirstLineAsync(hkcu);

        var hklm = FileHelper.TryGetFileInfo(Path.Combine(target.DirectoryName!, "hklm.reg"));
        await ExportAsync(RegistryType.HKEY_LOCAL_MACHINE.FastToString(), hklm);
        await RemoveFileFirstLineAsync(hklm);

        var hku = FileHelper.TryGetFileInfo(Path.Combine(target.DirectoryName!, "hku.reg"));
        await ExportAsync(RegistryType.HKEY_USERS.FastToString(), hku);
        await RemoveFileFirstLineAsync(hku);

        var hkcc = FileHelper.TryGetFileInfo(Path.Combine(target.DirectoryName!, "hkcc.reg"));
        await ExportAsync(RegistryType.HKEY_CURRENT_CONFIG.FastToString(), hkcc);
        await RemoveFileFirstLineAsync(hkcc);

        await FileHelper.AppendToAsync(target, hkcr, hkcu, hklm, hku, hkcc);

        hkcr.Delete();
        hkcu.Delete();
        hklm.Delete();
        hku.Delete();
        hkcc.Delete();
    }

    private static async Task RemoveFileFirstLineAsync(FileInfo file)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            // 以读取模式打开源文件
            using(var sourceStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // 以写入模式打开临时文件
                using var tempStream = new FileStream(tempFile, FileMode.CreateNew, FileAccess.Write);
                // 使用StreamReader读取第一行，不写入
                using(var reader = new StreamReader(sourceStream))
                {
                    await reader.ReadLineAsync();
                }

                // 使用BufferedStream提高复制效率
                using var bufferedStream = new BufferedStream(sourceStream, 64 * 1024); // 使用80KB缓冲区
                                                                                        // 将剩余内容从源文件复制到目标文件
                await bufferedStream.CopyToAsync(tempStream);
            }

            // 删除原文件
            file.Delete();

            // 将临时文件重命名为原文件名
            File.Move(tempFile, file.FullName);
        }
        catch(Exception ex)
        {
            Console.WriteLine("发生错误: " + ex.Message);
            try
            {
                // 如果发生错误，尝试删除临时文件
                File.Delete(tempFile);
            }
            catch { }
        }
    }
}
