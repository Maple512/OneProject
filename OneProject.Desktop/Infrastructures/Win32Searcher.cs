namespace OneProject.Desktop.Infrastructures;

using System.Management;
using OneProject.Desktop.ViewModels;

[FastEnum.Extensions]
public enum Win32TableEnum
{
    Win32_VideoController,
    Win32_DiskDrive,
    Win32_PhysicalMemory,
    Win32_Processor,
    Win32_BaseBoard,
    Win32_LogicalDisk,
}

public static class Win32Searcher
{
    /// <summary>
    /// GPU
    /// <para>参数：<see href="https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-videocontroller"/></para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Win32PropertyModel>> GetVideoController()
        => Query(Win32TableEnum.Win32_VideoController);

    /// <summary>
    /// 物理磁盘
    /// <para>参数：<see href="https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-diskdrive"/></para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Win32PropertyModel>> GetDiskDrive()
        => Query(Win32TableEnum.Win32_DiskDrive);

    /// <summary>
    /// 内存
    /// <para>参数：<see href="https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-physicalmemory"/></para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Win32PropertyModel>> GetPhysicalMemory()
        => Query(Win32TableEnum.Win32_PhysicalMemory);

    /// <summary>
    /// CPU
    /// <para>参数：<see href="https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-processor"/></para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Win32PropertyModel>> GetProcessor()
        => Query(Win32TableEnum.Win32_Processor);

    /// <summary>
    /// 主板
    /// <para>参数：<see href="https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-baseboard"/></para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Win32PropertyModel>> GetBaseBoard()
        => Query(Win32TableEnum.Win32_BaseBoard);

    /// <summary>
    /// 磁盘的逻辑分区
    /// <para>参数：<see href="https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk"/></para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<Win32PropertyModel>> GetLogicalDisk()
        => Query(Win32TableEnum.Win32_LogicalDisk);

    private static IEnumerable<IEnumerable<Win32PropertyModel>> Query(Win32TableEnum table)
    {
        var result = new ManagementObjectSearcher($"select * from {table.FastToString()}").Get();

        foreach(var baseObject in result.Cast<ManagementObject>())
        {
            var properties = new List<Win32PropertyModel>(result.Count);

            foreach(var data in baseObject.Properties)
            {
                if(data.Value is null)
                {
                    continue;
                }

                if((data.Value is string s) && string.IsNullOrWhiteSpace(s))
                {
                    continue;
                }

                properties.Add(new Win32PropertyModel(data));
            }

            yield return properties;
        }
    }
}
