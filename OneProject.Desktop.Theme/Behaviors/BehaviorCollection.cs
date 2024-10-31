namespace OneProject.Desktop.Behaviors;

using Microsoft.Xaml.Behaviors;

public class BehaviorCollection : FreezableCollection<Behavior>
{
    protected override Freezable CreateInstanceCore() => new BehaviorCollection();
}
