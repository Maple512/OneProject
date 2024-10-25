namespace OneProject.Desktop.Win32Native;

using System;
using System.Runtime.InteropServices;

public static class Shell32
{
    public static readonly Guid Downloads = new("374DE290-123F-4565-9164-39C4925E467B");

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
#pragma warning disable SYSLIB1054 // 使用 “LibraryImportAttribute” 而不是 “DllImportAttribute” 在编译时生成 P/Invoke 封送代码
    private static extern int SHGetKnownFolderPath(
#pragma warning restore SYSLIB1054 // 使用 “LibraryImportAttribute” 而不是 “DllImportAttribute” 在编译时生成 P/Invoke 封送代码
        [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
        uint dwFlags,
        IntPtr hToken,
        out string pszPath);

    public static string GetDownloadsFolderPath()
    {
        _ = SHGetKnownFolderPath(Downloads, 0, IntPtr.Zero, out var folder);

        return folder;
    }
}
