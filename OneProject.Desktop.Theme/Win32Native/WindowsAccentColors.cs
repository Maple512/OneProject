namespace OneProject.Desktop.Win32Native;

using System.Runtime.InteropServices;

public static class WindowsThemeManger
{
    private const string ColorName = "SystemAccent";
    private static uint? _colorCount;
    private static uint? _userIndex;

    public static Color GetCurrentThemeColor() => GetColor(ColorName);

    /// <summary>
    /// 获取颜色数量，同时也代表索引
    /// </summary>
    /// <returns></returns>
    private static uint GetCount()
    {
        _colorCount ??= PInvoke.GetImmersiveColorSetCount();

        return _colorCount.Value;
    }

    /// <summary>
    /// 获取用户当前的颜色索引
    /// </summary>
    /// <returns></returns>
    private static uint GetUserColor()
    {
        if(_userIndex is null)
        {
            _userIndex = PInvoke.GetImmersiveUserColorSetPreference(false, false);

            if(_userIndex > GetCount())
            {
                _userIndex = _colorCount! - 1;
            }
        }

        return _userIndex.Value;
    }

    private static Color GetColor(string colorName)
    {
        var index = GetUserColor();

        var type = GetColorType(colorName);

        //PInvoke.GetWindowTheme_SafeHandle(HWND.HWND_DESKTOP);

        //PInvoke.dw

        var nativeColor = PInvoke.GetImmersiveColorFromColorSetEx(
                index,
                type,
                false,
                0);

        return Color.FromArgb(
            (byte)((0xFF000000 & nativeColor) >> 24),
            (byte)((0x000000FF & nativeColor) >> 0),
            (byte)((0x0000FF00 & nativeColor) >> 8),
            (byte)((0x00FF0000 & nativeColor) >> 16)
        );
    }

    /// <summary>
    /// 通过暴力尝试操作系统函数来收集所有可用的颜色名称。由于目前没有已知的方法来检索所有可能的颜色名称，
    /// 以下方法只是尝试所有从 0 到 0xFFF 的索引，忽略错误。
    /// </summary>
    /// <returns></returns>
    //public static List<string> GetAllColorNames()
    //{
    //    var result = new List<string>(0xFFF);

    //    for(uint i = 0; i < 0xFFF; i++)
    //    {
    //        var typeNamePtr = UXTheme.GetImmersiveColorNamedTypeByIndex(i);
    //        if(typeNamePtr == nint.Zero)
    //        {
    //            continue;
    //        }

    //        var typeName = Marshal.PtrToStructure<nint>(typeNamePtr);

    //        result.Add(Marshal.PtrToStringUni(typeName)!);
    //    }

    //    return result;
    //}

    /// <summary>
    /// 根据颜色名称获得索引
    /// </summary>
    /// <param name="colorName"></param>
    /// <returns></returns>
    private static uint GetColorType(string colorName)
    {
        var name = nint.Zero;

        try
        {
            name = Marshal.StringToHGlobalUni("Immersive" + colorName);

            return PInvoke.GetImmersiveColorTypeFromName(name);
        }
        finally
        {
            if(name != nint.Zero)
            {
                Marshal.FreeHGlobal(name);
            }
        }
    }
}
