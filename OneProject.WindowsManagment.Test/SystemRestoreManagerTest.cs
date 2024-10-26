namespace OneProject.WindowsManagment.Test;

using System.Text.Json;

public class SystemRestoreManagerTest : TestBase<SystemRestoreManagerTest>
{
    [Fact]
    public void create_and_query()
    {
        var description = DateTime.Now.ToString("mmssfff");
        var result = SystemRestoreManager.Create(description);

        var points = SystemRestoreManager.Query();

        points.Any(x => x.Description?.Contains(description) == true)
            .ShouldBeTrue();
    }
}
