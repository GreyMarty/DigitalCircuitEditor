namespace Editor.Component;

public abstract class ComponentBase
{
    public IEntity Entity { get; private set; } = default!;
    public IContext Context => Entity.Context;
    
    public bool Initialized { get; private set; }


    internal void Init(IEntity entity)
    {
        if (Initialized)
        {
            return;
        }

        Entity = entity;
        
        OnInit();
        Initialized = true;
    }

    internal void Destroy()
    {
        if (!Initialized)
        {
            return;
        }
        
        OnDestroy();
        Initialized = false;
    }
    
    protected virtual void OnInit() { }
    protected virtual void OnDestroy() { }
}

public abstract class ComponentBase<TContext> : ComponentBase 
    where TContext : IContext
{
    public new TContext Context => (TContext)base.Context;
}