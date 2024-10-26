namespace OneProject.Desktop.Infrastructures;

using CliWrap;

public static class CliExtensions
{
    public static Command WithArguments(this Command command, params string[] args) => command.WithArguments(args);
}
