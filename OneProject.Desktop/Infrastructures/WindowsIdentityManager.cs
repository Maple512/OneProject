namespace OneProject.Desktop.Infrastructures;

using System.Security.Principal;

public static class WindowsIdentityManager
{
    public static bool IsCurrentUserAdministrator()
    {
        var user = WindowsIdentity.GetCurrent();

        var principal = new WindowsPrincipal(user);

        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static void RequestAdministratorRights()
    {
        try
        {
            var proc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Environment.ProcessPath,
                Verb = "runas"
            };

            Process.Start(proc);
        }
        catch
        {
            // 处理异常，例如用户拒绝提升权限
        }
    }
}
