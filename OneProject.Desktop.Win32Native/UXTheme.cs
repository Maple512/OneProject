namespace OneProject.Desktop.Win32Native;

using System.Runtime.InteropServices;

public partial class UXTheme
{
    [LibraryImport("uxtheme.dll", EntryPoint = "#98")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
    public static partial uint GetImmersiveUserColorSetPreference([MarshalAs(UnmanagedType.Bool)] bool forceCheckRegistry, [MarshalAs(UnmanagedType.Bool)] bool skipCheckOnFail);

    [LibraryImport("uxtheme.dll", EntryPoint = "#94")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
    public static partial uint GetImmersiveColorSetCount();

    [LibraryImport("uxtheme.dll", EntryPoint = "#95")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
    public static partial uint GetImmersiveColorFromColorSetEx(uint immersiveColorSet, uint immersiveColorType,
        [MarshalAs(UnmanagedType.Bool)] bool ignoreHighContrast, uint highContrastCacheMode);

    [LibraryImport("uxtheme.dll", EntryPoint = "#96")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
    public static partial uint GetImmersiveColorTypeFromName(nint name);

    [LibraryImport("uxtheme.dll", EntryPoint = "#100")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
    public static partial nint GetImmersiveColorNamedTypeByIndex(uint index);
}
