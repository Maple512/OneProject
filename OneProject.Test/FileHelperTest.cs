namespace OneProject;
using System.IO;

public class FileHelperTest
{
    [Fact]
    public void get_file_info()
    {
        var file = Path.GetRandomFileName();

        using(var fs = File.Create(file))
        {
            File.Exists(file).ShouldBeTrue();
        }

        var fi = FileHelper.TryGetFileInfo(file);
        fi.ShouldNotBeNull();
        fi.Name.ShouldBe(file);

        var fi1 = FileHelper.TryGetFileInfo(file, false);
        fi1.Exists.ShouldBeFalse();
        fi1.Name.ShouldContain("(1)");

        fi.Delete();
    }
}
