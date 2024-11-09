namespace Windows.Win32;

using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Gdi;

internal partial class PInvoke
{
    #region UXTheme
    /// <summary>
    /// 获取沉浸式颜色集合的数量
    /// </summary>
    /// <returns></returns>
    [LibraryImport("uxtheme.dll", EntryPoint = "#94")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
    public static partial uint GetImmersiveColorSetCount();

    /// <summary>
    /// 从指定的颜色集合中获取沉浸式颜色
    /// </summary>
    /// <param name="colorSet">参数指定颜色集合的索引</param>
    /// <param name="colorType"></param>
    /// <param name="ignoreHighContrast"></param>
    /// <param name="highContrastCacheMode"></param>
    /// <returns></returns>
    [LibraryImport("uxtheme.dll", EntryPoint = "#95")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
    public static partial uint GetImmersiveColorFromColorSetEx(uint colorSet, uint colorType, [MarshalAs(UnmanagedType.Bool)] bool ignoreHighContrast, uint highContrastCacheMode);

    /// <summary>
    /// 根据名称获取沉浸式颜色类型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [LibraryImport("uxtheme.dll", EntryPoint = "#96")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
    public static partial uint GetImmersiveColorTypeFromName(nint name);

    /// <summary>
    /// 获取当前用户的首选沉浸式颜色集合
    /// </summary>
    /// <returns></returns>
    [LibraryImport("uxtheme.dll", EntryPoint = "#98")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
    public static partial uint GetImmersiveUserColorSetPreference([MarshalAs(UnmanagedType.Bool)] bool forceCheckRegistry,
        [MarshalAs(UnmanagedType.Bool)] bool skipCheckOnFail);

    /// <summary>
    /// 根据索引获取沉浸式颜色的名称
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    [LibraryImport("uxtheme.dll", EntryPoint = "#100")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
    public static partial nint GetImmersiveColorNamedTypeByIndex(uint index);
    #endregion

    public static unsafe MONITORINFO GetMonitorInfo(IntPtr monitor)
    {
        MONITORINFO monitorInfo = default;

        monitorInfo.cbSize = (uint)Marshal.SizeOf<MONITORINFO>();

        if(GetMonitorInfo(new HMONITOR(monitor), ref monitorInfo))
        {
            return monitorInfo;
        }

        return default;
    }
}
