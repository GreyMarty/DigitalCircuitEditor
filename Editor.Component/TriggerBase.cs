namespace Editor.Component;

public interface ITrigger<out TArgs> where TArgs : ITriggerArgs
{
    public TriggerFilterMode FilterMode { get; }
    public IEnumerable<ITriggerFilter> Filters { get; }

    public event Action<TArgs>? Fired;


    public void Init(IEntity entity);
    public void Destroy();
}

public interface ITriggerArgs
{
}

public record TriggerArgs() : ITriggerArgs
{
    public static TriggerArgs Empty = new();
}

public abstract class TriggerBase<TContext, TArgs> : ComponentBase<TContext>, ITrigger<TArgs>
    where TContext : IContext where TArgs : ITriggerArgs
{
    public TriggerFilterMode FilterMode { get; init; }
    public TriggerFilterBase<TContext>[] Filters { get; init; } = Array.Empty<TriggerFilterBase<TContext>>();
    IEnumerable<ITriggerFilter> ITrigger<TArgs>.Filters => Filters;


    public event Action<TArgs>? Fired;


    public new void Init(IEntity entity)
    {
        base.Init(entity);
        
        foreach (var triggerFilter in Filters)
        {
            triggerFilter.Init(entity);
        }
    }

    public new void Destroy()
    {
        foreach (var triggerFilter in Filters)
        {
            triggerFilter.Destroy();
        }
        
        base.Destroy();
    }

    protected virtual void OnFired(TArgs e)
    {
        var canFire = FilterMode switch
        {
            TriggerFilterMode.Any => Filters.Length == 0 || Filters.Any(x => x.CanFire() ^ x.Invert),
            TriggerFilterMode.All => Filters.Length == 0 || Filters.All(x => x.CanFire() ^ x.Invert),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (canFire)
        {
            Fired?.Invoke(e);
        }
    }
}