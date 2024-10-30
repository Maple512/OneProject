namespace OneProject.Desktop.Test;

using OneProject.Desktop.Infrastructures;

public class WindowsRegistryManagerTest
{
    [Fact]
    public void search()
    {
        WindowsIdentityManager.IsCurrentUserAdministrator();

        WindowsIdentityManager.RequestAdministratorRights();

        var result = WindowsRegistryManager.Search("使用 Visual Studio 打开");

        result.Count.ShouldBeGreaterThan(0);
    }
}
