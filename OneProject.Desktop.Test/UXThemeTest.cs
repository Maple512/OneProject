namespace OneProject.Desktop.Test;

using Microsoft.Extensions.Logging;
using OneProject.Desktop.Win32Native;

public class UXThemeTest:TestBase<UXThemeTest>
{
    [Fact]
    public void Test1()
    {
        var colors = AccentColorSet.ActiveSet.GetAllColorNames();

        //foreach(var color in colors.Where(x => x.StartsWith("System")))
        //{
        //    Logger.LogDebug($"{color}: {AccentColorSet.ActiveSet[color]}");
        //}

        var color = AccentColorSet.ActiveSet["SystemAccent"];
        //var backgroundDark = AccentColorSet.ActiveSet["SystemBackgroundDarkTheme"];
        //var text = AccentColorSet.ActiveSet["SystemText"];
        //var accent = AccentColorSet.ActiveSet["SystemAccentLight3"];

        //var colorSetCount = UXTheme.GetImmersiveColorSetCount();

        //var colorSets = new List<AccentColorSet>();
        //for(uint i = 0; i < colorSetCount; i++)
        //{
        //    colorSets.Add(new AccentColorSet(i, false));
        //}

        //var color = UXTheme.GetImmersiveUserColorSetPreference(false, false);
    }
}
