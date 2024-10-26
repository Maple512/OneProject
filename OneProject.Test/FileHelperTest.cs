namespace OneProject;

using System.IO;
using System.Text;

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

        var fi = FileHelper.TryGetFileInfo(file, true);
        fi.ShouldNotBeNull();
        fi.Name.ShouldBe(file);

        var fi1 = FileHelper.TryGetFileInfo(file);
        fi1.Exists.ShouldBeFalse();
        fi1.Name.ShouldContain("(1)");

        fi.Delete();
    }

    [Fact]
    public async Task append_to()
    {
        var folder = "a_temps";

        if(Directory.Exists(folder) == false)
        {
            Directory.CreateDirectory(folder);
        }

        var target = Path.Combine(folder, Path.GetRandomFileName());

        var source = new List<FileInfo>(20);

        using(var targetFile = File.Create(target))
        {
            await targetFile.WriteAsync(Encoding.UTF8.GetBytes(RandomHelper.GetString(100)));

            foreach(var item in Enumerable.Range(0, 20))
            {
                using var file = File.Create(Path.Combine(folder, Path.GetRandomFileName()));

                await file.WriteAsync(Encoding.UTF8.GetBytes(RandomHelper.GetString(100)));

                source.Add(new FileInfo(file.Name));
            }
        }

        var fi = new FileInfo(target);

        await FileHelper.AppendToAsync(fi, source);

        var nlLenght = Encoding.UTF8.GetByteCount(Environment.NewLine) * 20;

        fi.Length.ShouldBe(2100 + nlLenght);
    }
}
