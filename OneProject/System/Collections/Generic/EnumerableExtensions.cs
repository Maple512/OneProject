namespace System.Collections.Generic;

using OneProject;

[StackTraceHidden]
[DebuggerStepThrough]
public static class EnumerableExtensions
{
    public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
    {
        Check.NotNull(source);

        return string.Join(separator, source);
    }

    public static string JoinAsString<T>(this IEnumerable<T> source, char separator = ',')
    {
        Check.NotNull(source);

        return string.Join(separator, source);
    }
}
