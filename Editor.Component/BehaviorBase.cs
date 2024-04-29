namespace Editor.Component;

public interface IBehavior<TArgs> where TArgs : ITriggerArgs
{
    public ITrigger<TArgs>[] Triggers { get; init; }
}

public abstract class BehaviorBase<TContext, TArgs> : ComponentBase<TContext>, IBehavior<TArgs>
    where TContext : IContext where TArgs : ITriggerArgs
{
    public ITrigger<TArgs>[] Triggers { get; init; } = Array.Empty<ITrigger<TArgs>>();

    protected override void OnInit()
    {
        foreach (var trigger in Triggers)
        {
            trigger.Fired += Perform;
            trigger.Init(Entity);
        }
    }

    protected override void OnDestroy()
    {
        foreach (var trigger in Triggers)
        {
            trigger.Destroy();
            trigger.Fired -= Perform;
        }
    }

    protected abstract void Perform(TArgs e);
}

public abstract class BehaviorBase<TContext> : BehaviorBase<TContext, ITriggerArgs>
    where TContext : IContext
{
    protected sealed override void Perform(ITriggerArgs e)
    {
        Perform();
    }

    protected abstract void Perform();
}