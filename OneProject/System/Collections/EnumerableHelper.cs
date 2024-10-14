namespace System.Collections;

[StackTraceHidden]
[DebuggerStepThrough]
public static class EnumerableHelper
{
    public static bool Any(IEnumerable? source)
    {
        if(source is null)
        {
            return false;
        }

        if(source is ICollection c)
        {
            return c.Count > 0;
        }

        var enumerator = source.GetEnumerator();

        try
        {
            return enumerator.MoveNext();
        }
        finally
        {
            enumerator.Reset();

            (enumerator as IDisposable)?.Dispose();
        }
    }
}
