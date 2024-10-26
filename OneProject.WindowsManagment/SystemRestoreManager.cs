namespace OneProject.WindowsManagment;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using FastEnum;

[Extensions]
public enum EventTypeEnum
{
    [Description("系统更改已经开始")]
    BEGIN_SYSTEM_CHANGE = 100,
    [Description("系统更改已结束")]
    END_SYSTEM_CHANGE = 101,
    [Description("系统更改已经开始，后续的嵌套调用不会创建新的还原点")]
    BEGIN_NESTED_SYSTEM_CHANGE = 102,
    [Description("系统更改已结束")]
    END_NESTED_SYSTEM_CHANGE = 103,
}

[Extensions]
public enum RestorePointTypeEnum
{
    [Description("已安装应用程序")]
    APPLICATION_INSTALL = 0,

    [Description("应用程序已卸载")]
    APPLICATION_UNINSTALL = 1,

    [Description("已安装设备驱动程序")]
    DEVICE_DRIVER_INSTALL = 10,

    [Description("应用程序已添加或删除功能")]
    MODIFY_SETTINGS = 12,

    [Description("应用程序需要删除它创建的还原点")]
    CANCELLED_OPERATION = 13,
}

public static partial class SystemRestoreManager
{
    /// <summary>
    /// 创建系统还原点
    /// <para>docs: <see href="https://learn.microsoft.com/zh-cn/windows/win32/sr/createrestorepoint-systemrestore"/></para>
    /// </summary>
    public static void Create(string? description = null)
    {
        var manager = new ManagementClass("\\root\\default", "SystemRestore", null);

        var args = manager.GetMethodParameters("CreateRestorePoint");
        args["Description"] = $"OneProject: {description}";
        args["RestorePointType"] = 0;
        args["EventType"] = 100;

        manager.InvokeMethod("CreateRestorePoint", args, new InvokeMethodOptions(null, TimeSpan.MaxValue));
    }

    public static List<SystemRestoreModel> Query()
    {
        var searcher = new ManagementObjectSearcher("root\\default",
            "SELECT * FROM SystemRestore");

        var result = searcher.Get();
        var models = new List<SystemRestoreModel>(result.Count);

        foreach(var restorePoint in result.Cast<ManagementObject>())
        {
            var date = DateTimeOffset.ParseExact((string)restorePoint["CreationTime"], "yyyyMMddHHmmss.ffffff-000", CultureInfo.InvariantCulture,
                                              DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            _ = EventTypeEnumExtensions.TryParse(restorePoint["EventType"].ToString(), out var eventType);
            _ = RestorePointTypeEnumExtensions.TryParse(restorePoint["RestorePointType"].ToString(), out var restorePointType);

            models.Add(new SystemRestoreModel
            {
                SequenceNumber = (uint)restorePoint["SequenceNumber"],
                Description = (string)restorePoint["Description"],
                CreationTime = date,
                EventType = eventType,
                RestorePointType = restorePointType,
            });
        }

        return models;
    }

    // 可以提醒去系统保护中使用还原功能
    //public static void Restore(uint sequence)
    //{
    //    var restore = new ManagementClass("\\root\\default", "SystemRestore", null);

    //    var paras = restore.GetMethodParameters("Restore");

    //    paras["SequenceNumber"] = sequence;

    //    restore.InvokeMethod("Restore", paras, null);
    //}

    public static void Delete(uint sequence)
    {
        SRRemoveRestorePoint(sequence);
    }

    // see: https://learn.microsoft.com/zh-cn/windows/win32/api/srrestoreptapi/nf-srrestoreptapi-srremoverestorepoint
    [LibraryImport("SrClient.dll")]
    private static partial int SRRemoveRestorePoint(uint dwSequenceNumber);
}

public class SystemRestoreModel
{
    private string? event_description;
    private string? restore_point_description;

    public uint SequenceNumber { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset CreationTime { get; init; }
    public EventTypeEnum EventType { get; init; }
    public string? Event_Description => event_description ??= EventType.GetDescription();
    public RestorePointTypeEnum RestorePointType { get; init; }
    public string? RestorePointType_Description => restore_point_description ??= RestorePointType.GetDescription();
}
