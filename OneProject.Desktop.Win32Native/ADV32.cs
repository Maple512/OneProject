namespace OneProject.Desktop.Win32Native;

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public partial class ADV32
{
    [DllImport("advapi32.dll", EntryPoint = "RegSaveKeyEx", SetLastError = true)]
    private static extern int RegSaveKeyEx(
        IntPtr hKey,
        string lpFile,
        IntPtr lpSecurityAttributes,
        int Flags
    );

    public static void ExportRegistryKeyToBytes(RegistryKey key,FileInfo fi)
    {
        // 将RegistryKey句柄转换为IntPtr
        IntPtr handle = key.Handle.DangerousGetHandle();

        // 调用RegSaveKeyEx函数导出注册表项
        int result = RegSaveKeyEx(handle, fi.FullName, IntPtr.Zero, 0);

        if(result != 0)
        {
            throw new InvalidOperationException("Failed to export registry key. Error code: " + result);
        }
    }

    public enum HKEY : uint
    {
        HKEY_CLASSES_ROOT = 0x80000000,
        HKEY_CURRENT_USER = 0x80000001,
        HKEY_LOCAL_MACHINE = 0x80000002,
        HKEY_USERS = 0x80000003,
        HKEY_PERFORMANCE_DATA = 0x80000004,
        HKEY_PERFORMANCE_TEXT = 0x80000050,
        HKEY_PERFORMANCE_NLSTEXT = 0x80000060,
        HKEY_CURRENT_CONFIG = 0x80000005
    }

    [DllImport("advapi32.dll", EntryPoint = "RegOpenKey", SetLastError = true)]
    internal static extern int RegOpenKeyA(uint hKey, string lpSubKey, ref int phkResult);

    [DllImport("advapi32.dll", EntryPoint = "RegReplaceKey", SetLastError = true)]
    internal static extern int RegReplaceKeyA(int hKey, string lpSubKey, string lpNewFile, string lpOldFile);

    [DllImport("advapi32.dll", EntryPoint = "RegSaveKey", SetLastError = true)]
    internal static extern int RegSaveKeyA(int hKey, string lpFile, int lpSecurityAttributes);

    [DllImport("advapi32.dll")]
    internal static extern int RegCloseKey(int hKey);

    [DllImport("advapi32.dll")]
    internal static extern int RegFlushKey(int hKey);

    public static int Open(string name)
    {
        var result = 0;

        _ = RegOpenKeyA((uint)HKEY.HKEY_CLASSES_ROOT, name, ref result);

        return result;
    }

    public static bool BackupRegstry(RegistryKey key, FileInfo file)
    {
        //var handle = key.Handle.DangerousGetHandle();

        var result = RegSaveKeyA(3594, file.FullName, 0);

        return result == 0;
    }
}
