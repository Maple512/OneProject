namespace OneProject.Desktop.Win32Native;

public static class DirectoryHelper
{
    static readonly Guid Downloads = new("374DE290-123F-4565-9164-39C4925E467B");

    //public static string GetDownloadsFolderPath()
    //{
    //    var result = PInvoke.SHGetKnownFolderPath(Downloads, KNOWN_FOLDER_FLAG.KF_FLAG_DEFAULT, null, out var path);

    //    result.ThrowOnFailure();

    //    return path.ToString();
    //}
}
