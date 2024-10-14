namespace OneProject;

public class Interlocker(bool locked = false)
{
    private int _locked = locked ? 1 : 0;

    public bool IsLocked => Interlocked.CompareExchange(ref _locked, 1, 0) != 0;

    public void Lock() => Interlocked.Exchange(ref _locked, 1);

    public void Unlock() => Interlocked.Exchange(ref _locked, 0);
}
