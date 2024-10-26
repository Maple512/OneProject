namespace OneProject.Desktop.Infrastructures;

using System;
using System.Management;
using System.Text.Json;

public static class SystemRestorePointHelper
{
    public static void CreateRestorePoint(string description)
    {
        // 获取WMI服务
        //var scope = new ManagementScope(@"\root\default");
        //var systemRestoreClass = new ManagementClass(scope, new ManagementPath("SystemRestore"), null);

        //// 设置方法参数
        //var inParams = systemRestoreClass.GetMethodParameters("CreateRestorePoint");
        //inParams["Description"] = description;
        //inParams["EventType"] = 100; // APPLICATION_INSTALL = 100
        //inParams["RestorePointType"] = 0; // BEGIN_SYSTEM_CHANGE = 0

        //// 调用方法
        //var outParams = systemRestoreClass.InvokeMethod("CreateRestorePoint", inParams, null);

        //// 检查返回值
        //if((uint)outParams["ReturnValue"] != 0)
        //{
        //    throw new Exception("Failed to create restore point.");
        //}

        //ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\default", "SELECT * FROM SystemRestore");
        //foreach(ManagementObject restorePoint in searcher.Get())
        //{
        //    var content = JsonSerializer.Serialize(restorePoint);

        //    //Console.WriteLine("Restore Point Sequence Number: " + restorePoint["SequenceNumber"]);
        //    //Console.WriteLine("Restore Point Description: " + restorePoint["Description"]);
        //    // 其他属性...
        //}
    }
}
