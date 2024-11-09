namespace OneProject.Desktop.Infrastructures;

using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Threading.Tasks;
using CliWrap;
using FastEnum;
using Microsoft.Win32;

[Extensions]
public enum RegistryType
{
    HKEY_CLASSES_ROOT,
    HKEY_CURRENT_USER,
    HKEY_LOCAL_MACHINE,
    HKEY_USERS,
    HKEY_CURRENT_CONFIG,
}

public class WindowsRegistryManager
{

    /*
文件右键

[HKEY_CLASSES_ROOT\*\shell\Open with JetBrains Rider]
"Icon"="D:\\Rider\\bin\\rider64.exe"
@="Edit with JetBrains Rider"

[HKEY_CLASSES_ROOT\*\shell\Open with JetBrains Rider\command]
@="\"D:\\Rider\\bin\\rider64.exe\" \"%1\""

目录（或桌面）右键
[HKEY_CLASSES_ROOT\Directory\Background\shell\搜索 Everything...]
"Icon"="E:\\Everything\\Everything.exe,0"

[HKEY_CLASSES_ROOT\Directory\Background\shell\搜索 Everything...\command]
@="\"E:\\Everything\\Everything.exe\" -path \"%V\""

    [HKEY_CLASSES_ROOT\Directory\shell\PotPlayer.Enqueue]
@="添加到 PotPlayer 列表(&U)"
"Icon"="E:\\PotPlayer\\PotPlayerMini64.exe"

[HKEY_CLASSES_ROOT\Directory\shell\PotPlayer.Enqueue\Command]
@="\"E:\\PotPlayer\\PotPlayerMini64.exe\" \"%1\"  /Add"

[HKEY_CLASSES_ROOT\Directory\shell\PotPlayer.Enqueue\DropTarget]
"Clsid"="{6712D17C-CEB6-4886-9641-427AF3D488B7}"
    //using var key = Registry.ClassesRoot.OpenSubKey("\\*\\shell\\Open with JetBrains Rider");

        //var names = key.GetValueNames();

        //var kind = key.GetValueKind("");

        //var value1 = key.GetValue("");
        //var value2 = key.GetValue("Icon");

        //var subKeys = key.GetSubKeyNames();

        //var command = key.OpenSubKey("command").GetValue("");
*/
    public static void AddToDirectory(string name, string icon, string command)
    {
        using var key = Registry.ClassesRoot.CreateSubKey("\\Directory\\Background\\shell", true);

        var subKeys = key.GetSubKeyNames();

        RegistryKey subKey;
        if(subKeys.Contains(name))
        {
            subKey = key.OpenSubKey(name, true)!;
        }
        else
        {
            subKey = key.CreateSubKey(name, true);
        }

        subKey.SetValue(string.Empty, name);
        subKey.SetValue("Icon", icon);

        RegistryKey commandKey;
        if(subKeys.Contains(name))
        {
            commandKey = key.OpenSubKey("command", true)!;
        }
        else
        {
            commandKey = key.CreateSubKey("command", true);
        }

        commandKey.SetValue(string.Empty, command);
    }

    public static void AddToFile(string name, string icon, string command)
    {
        using var key = Registry.ClassesRoot.CreateSubKey("\\*\\shell", true);

        var subKeys = key.GetSubKeyNames();

        RegistryKey subKey;
        if(subKeys.Contains(name))
        {
            subKey = key.OpenSubKey(name, true)!;
        }
        else
        {
            subKey = key.CreateSubKey(name, true);
        }

        subKey.SetValue(string.Empty, name);
        subKey.SetValue("Icon", icon);

        RegistryKey commandKey;
        if(subKeys.Contains(name))
        {
            commandKey = key.OpenSubKey("command", true)!;
        }
        else
        {
            commandKey = key.CreateSubKey("command", true);
        }

        commandKey.SetValue(string.Empty, command);
    }

    public static Dictionary<string, object?> Search(string pattern)
    {
        var result = SearchRegistry(Registry.ClassesRoot, pattern);
        result = result.Concat(SearchRegistry(Registry.CurrentUser, pattern));
        result = result.Concat(SearchRegistry(Registry.LocalMachine, pattern));
        result = result.Concat(SearchRegistry(Registry.Users, pattern));
        result = result.Concat(SearchRegistry(Registry.CurrentConfig, pattern));

        return result.ToDictionary(x => x.Key, x => x.Value);
    }

    // 搜索注册表项、值和数据
    public static IEnumerable<KeyValuePair<string, object?>> SearchRegistry(RegistryKey key, string searchPattern)
    {
        IEnumerable<KeyValuePair<string, object?>> result = null!;
        try
        {
            // 搜索项
            result = SearchRegistryKeys(key, searchPattern);

            // 搜索值
            result = result.Concat(SearchRegistryValues(key, searchPattern));

            key.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error searching the registry: {ex.Message}");
        }

        return result;
    }

    private static string[]? GetSubKeyNames(RegistryKey key)
    {
        try
        {
            return key.GetSubKeyNames();
        }
        catch
        {
            return null;
        }
    }

    private static string[]? GetValueNames(RegistryKey key)
    {
        try
        {
            return key.GetValueNames();
        }
        catch
        {
            return null;
        }
    }

    private static IEnumerable<KeyValuePair<string, object?>> SearchRegistryKeys(RegistryKey key, string searchPattern)
    {
        var names = GetSubKeyNames(key);

        if(names is null)
        {
            var valueNames = GetValueNames(key);

            if(valueNames is null or { Length: 0 })
            {
                yield break;
            }

            foreach(var valueName in valueNames)
            {
                var value = key.GetValue(valueName);

                if(value is null)
                {
                    yield break;
                }

                if(value.ToString()?.Contains(searchPattern, StringComparison.OrdinalIgnoreCase) == true)
                {
                    Log.Information($"Value: {value}");

                    yield return new KeyValuePair<string, object?>(key.Name, value);
                }
            }

            yield break;
        }

        foreach(var subKeyName in names)
        {
            Log.Information($"Key: {subKeyName}");

            if(subKeyName.Contains(searchPattern) || string.IsNullOrEmpty(searchPattern))
            {
                var value = key.GetValue(subKeyName);

                Log.Information($"Value: {value}");

                yield return new KeyValuePair<string, object?>(subKeyName, value);
            }

            RegistryKey? subKey = null;
            try
            {
                // 递归搜索子项
                subKey = key.OpenSubKey(subKeyName, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.QueryValues);
            }
            catch(SecurityException) // Requested registry access is not allowed
            {
                yield break;
            }

            if(subKey != null)
            {
                var value = key.GetValue(subKeyName);

                if(value != null)
                {

                }

                foreach(var subKeyItem in SearchRegistryKeys(subKey, searchPattern))
                {
                    yield return subKeyItem;
                }

                subKey.Close();
            }
        }
    }

    private static IEnumerable<KeyValuePair<string, object?>> SearchRegistryValues(RegistryKey key,
        string searchPattern)
    {
        foreach(var subKey in key.GetValueNames())
        {
            var value = key.GetValue(subKey);

            Serilog.Log.Information($"Value: {value}");

            if(value?.ToString()?.Contains(searchPattern) == true)
            {
                yield return new KeyValuePair<string, object?>(subKey, value);
            }
        }
    }

    public static void AddExplorerContextMenu()
    {
        // 打开注册表项
        var baseKey = Registry.CurrentUser.OpenSubKey("Software\\Classes", true);
        var shellKey = baseKey.CreateSubKey("*\\shell\\OpenWithVSCode");

        // 设置菜单项名称
        shellKey.SetValue("", "使用 VS Code 打开文件");

        // 创建 command 子键
        var commandKey = shellKey.CreateSubKey("command");
        commandKey.SetValue("", @"C:\Program Files\Microsoft VS Code\Code.exe %1");

        // 关闭注册表项
        baseKey.Close();
    }

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
        var dir = Path.GetTempPath();

        var hkcr = FileHelper.TryGetFileInfo(Path.Combine(dir, "hkcr.reg"));
        await ExportAsync(RegistryType.HKEY_CLASSES_ROOT.FastToString(), hkcr);
        await RemoveFileFirstLineAsync(hkcr);

        var hkcu = FileHelper.TryGetFileInfo(Path.Combine(dir, "hkcu.reg"));
        await ExportAsync(RegistryType.HKEY_CURRENT_USER.FastToString(), hkcu);
        await RemoveFileFirstLineAsync(hkcu);

        var hklm = FileHelper.TryGetFileInfo(Path.Combine(dir, "hklm.reg"));
        await ExportAsync(RegistryType.HKEY_LOCAL_MACHINE.FastToString(), hklm);
        await RemoveFileFirstLineAsync(hklm);

        var hku = FileHelper.TryGetFileInfo(Path.Combine(dir, "hku.reg"));
        await ExportAsync(RegistryType.HKEY_USERS.FastToString(), hku);
        await RemoveFileFirstLineAsync(hku);

        var hkcc = FileHelper.TryGetFileInfo(Path.Combine(dir, "hkcc.reg"));
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

    public class RegistryItemModel
    {
        public string? Key { get; }

        public string? Name { get; }

        public object? Value { get; }
    }
}
