namespace System.IO;

using OneProject;

[StackTraceHidden]
[DebuggerStepThrough]
public static class FileHelper
{
    /// <summary>
    /// 确保文件不存在，如果存在，覆盖原有文件或新建文件（文件名后加序号）
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cover">存在时是覆盖，还是新建，默认覆盖</param>
    /// <returns></returns>
    public static FileInfo TryGetFileInfo(string file, bool cover = true)
    {
        Check.NotNullOrWhiteSpace(file);

        var fi = new FileInfo(file);

        if(fi.Attributes is FileAttributes.Directory)
        {
            throw new ArgumentException($"The file path('{file}') is not a valid file address.", file);
        }

        var directory = fi.DirectoryName!;
        if(fi.Directory?.Exists != true)
        {
            Directory.CreateDirectory(directory);
        }

        if(!fi.Exists || cover)
        {
            return fi;
        }

        var i = 1;
        var name = Path.GetFileNameWithoutExtension(fi.FullName);
        while(fi.Exists)
        {
            fi = new(Path.Combine(directory, $"{name}({i++}){fi.Extension}"));
        }

        return fi;
    }
}
