namespace OneProject.Desktop.Test;

using System.Windows.Media;
using System.Windows;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // https://gist.github.com/paulcbetts/3c6aedc9f0cd39a77c37
        //var accentColor = new SolidColorBrush(AccentColorSet.ActiveSet["SystemAccent"]);
        //this.Code.Background = accentColor;
        //this.Code.Text = "AccentColorSet Immersive 'SystemAccent' " + accentColor.Color.ToString();

        // Available in .NET 4.5
        //var Background = SystemParameters.WindowGlassBrush;
        //var Text = "SystemParameters.WindowGlassBrush " + ((SolidColorBrush)SystemParameters.WindowGlassBrush).Color.ToString();
    }
}
