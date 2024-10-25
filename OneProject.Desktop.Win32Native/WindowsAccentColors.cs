namespace OneProject.Desktop.Win32Native;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Interop;

public class WindowsAccentColors
{
    //    internal static void SetupAccentsUpdate(MainWindow mainWindow)
    //    {
    //        [SupportedOSPlatform("windows")]
    //        void SystemEvents_UserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
    //        {
    //            UpdateAccentColor(mainWindow);
    //        }

    //        SystemEvents.UserPreferenceChanging += SystemEvents_UserPreferenceChanging;
    //    }

    //    public static void UpdateAccentColor(MainWindow mainWindow)
    //    {
    //#if false
    //            File.WriteAllLines(@"F:\Accents.txt", AccentColorSet.ActiveSet.GetAllColorNames().Select(s => {
    //                var c = AccentColorSet.ActiveSet[s];
    //                return $"{s}: {c}";
    //            }));
    //#endif
    //        var background = AccentColorSet.ActiveSet["SystemBackground"];
    //        var backgroundDark = AccentColorSet.ActiveSet["SystemBackgroundDarkTheme"];
    //        var shadow = background;
    //        var text = AccentColorSet.ActiveSet["SystemText"];
    //        var accent = AccentColorSet.ActiveSet["SystemAccentLight3"];
    //        //if (background != backgroundDark)
    //        //{
    //        //    accent = AccentColorSet.ActiveSet["SystemAccentDark3"];
    //        //}
    //        accent.A = 128;
    //        background.A = 160;

    //        if(UXTheme.MakeEdgesRounded(mainWindow))
    //        {
    //            mainWindow.SeparateFromTaskbar = true;
    //        }

    //        if(UXTheme.EnableBlur(mainWindow, background))
    //        {
    //            background = Colors.Transparent;
    //        }
    //        mainWindow.PopupBackground = new SolidColorBrush(background);
    //        mainWindow.AccentBackground = new SolidColorBrush(accent);
    //        mainWindow.TextColor = new SolidColorBrush(text);
    //        mainWindow.TextShadow = shadow;
    //    }


}

public class AccentColorSet
{
    private static AccentColorSet[]? _allSets;
    private static AccentColorSet? _activeSet;

    private readonly uint _colorSet;

    public AccentColorSet(uint colorSet, bool active)
    {
        _colorSet = colorSet;
        Active = active;
    }

    public static AccentColorSet[] AllSets
    {
        get
        {
            if(_allSets != null)
            {
                return _allSets;
            }

            var colorSetCount = UXTheme.GetImmersiveColorSetCount();

            var colorSets = new List<AccentColorSet>();
            for(uint i = 0; i < colorSetCount; i++)
            {
                colorSets.Add(new AccentColorSet(i, false));
            }

            AllSets = colorSets.ToArray();

            return _allSets;
        }
        private set => _allSets = value;
    }

    public static AccentColorSet ActiveSet
    {
        get
        {
            var activeSet = UXTheme.GetImmersiveUserColorSetPreference(false, false);
            ActiveSet = AllSets[Math.Min(activeSet, AllSets.Length - 1)];
            return _activeSet;
        }
        private set
        {
            if(_activeSet != null)
            {
                _activeSet.Active = false;
            }

            value.Active = true;
            _activeSet = value;
        }
    }

    public bool Active { get; private set; }

    public Color this[string colorName]
    {
        get
        {
            return this[GetColorType(colorName)];
        }
    }

    public Color this[uint colorType]
    {
        get
        {
            var nativeColor = UXTheme.GetImmersiveColorFromColorSetEx(_colorSet, colorType, false, 0);
            //if (nativeColor == 0)
            //    throw new InvalidOperationException();
            return Color.FromArgb(
                (byte)((0xFF000000 & nativeColor) >> 24),
                (byte)((0x000000FF & nativeColor) >> 0),
                (byte)((0x0000FF00 & nativeColor) >> 8),
                (byte)((0x00FF0000 & nativeColor) >> 16)
            );
        }
    }

    public uint GetColorType(string colorName)
    {
        var name = nint.Zero;
        uint colorType;

        try
        {
            name = Marshal.StringToHGlobalUni("Immersive" + colorName);
            colorType = UXTheme.GetImmersiveColorTypeFromName(name);
            return colorType;
        }
        finally
        {
            if(name != nint.Zero)
            {
                Marshal.FreeHGlobal(name);
            }
        }
    }

    public uint GetRawColor(uint colorType)
    {
        return UXTheme.GetImmersiveColorFromColorSetEx(_colorSet, colorType, false, 0);
    }
    public uint GetRawColor(string colorName)
    {
        return GetRawColor(GetColorType(colorName));
    }

    // HACK: GetAllColorNames collects the available color names by brute forcing the OS function.
    //   Since there is currently no known way to retrieve all possible color names,
    //   the method below just tries all indices from 0 to 0xFFF ignoring errors.
    public List<string> GetAllColorNames()
    {
        var allColorNames = new List<string>();
        for(uint i = 0; i < 0xFFF; i++)
        {
            var typeNamePtr = UXTheme.GetImmersiveColorNamedTypeByIndex(i);
            if(typeNamePtr == nint.Zero)
            {
                continue;
            }

            var typeName = (nint)Marshal.PtrToStructure(typeNamePtr, typeof(nint));
            allColorNames.Add(Marshal.PtrToStringUni(typeName));
        }

        return allColorNames;
    }
}
