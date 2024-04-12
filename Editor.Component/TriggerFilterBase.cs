namespace Editor.Component;

public interface ITriggerFilter
{
    public bool Invert { get; }
    
        
    public bool CanFire();
}

public enum TriggerFilterMode
{
    Any,
    All
}

public abstract class TriggerFilterBase<TContext> : ComponentBase<TContext>, ITriggerFilter 
    where TContext : IContext
{
    public bool Invert { get; init; }
    
    
    public abstract bool CanFire();
}