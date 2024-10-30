namespace OneProject.Desktop.Test;

using OneProject.Desktop.Infrastructures;

public class SystemRestoreManagerTest : TestBase<SystemRestoreManagerTest>
{
    [Fact]
    public void create_and_query()
    {
        var description = DateTime.Now.ToString("mmssfff");

        SystemRestoreManager.Create(description);

        var points = SystemRestoreManager.Query();

        points.Any(x => x.Description?.Contains(description) == true)
            .ShouldBeTrue();
    }
}
