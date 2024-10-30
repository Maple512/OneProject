namespace System.IO;

using System.Buffers;
using System.Text;
using OneProject;

[StackTraceHidden]
//[DebuggerStepThrough]
public static class FileHelper
{
    /// <summary>
    /// 确保文件不存在，如果存在，覆盖原有文件或新建文件（文件名后加序号）
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cover">存在时，加序号还是覆盖</param>
    /// <returns></returns>
    public static FileInfo TryGetFileInfo(string file, bool cover = false)
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

    /// <inheritdoc cref="FileHelper.AppendToAsync(FileInfo, IEnumerable{FileInfo}, bool)"/>
    public static async Task<long> AppendToAsync(FileInfo target, params FileInfo[] source)
    {
        return await AppendToAsync(target,  source as IEnumerable<FileInfo>);
    }

    /// <summary>
    /// 将所有给定源文件的内容附加到目标文件
    /// </summary>
    /// <param name="target">目标文件</param>
    /// <param name="source">源文件</param>
    /// <param name="newline">是否在每个源文件附加时换行</param>
    /// <returns></returns>
    public static async Task<long> AppendToAsync(FileInfo target, IEnumerable<FileInfo> source)
    {
        var total = 0L;

        var buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);

        try
        {
            var memory = buffer.AsMemory();

            using var writer = new FileStream(target.FullName, FileMode.Append, FileAccess.Write);

            foreach(var file in source)
            {
                using var reader = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);

                if(reader.Length == 0)
                {
                    continue;
                }

                var a = writer.Position;

                while(true)
                {
                    var read = await reader.ReadAsync(memory);
                    if(read == 0)
                    {
                        break;
                    }

                    total += read;
                    await writer.WriteAsync(memory[..read]);
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }

        return total;
    }

    public static void ThrowIfExisted(FileInfo file)
    {
        if(file?.Exists == true)
        {
            throw new NotSupportedException($"The file({file.Name}) is existed.");
        }
    }
}
