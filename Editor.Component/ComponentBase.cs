namespace Editor.Component;

public abstract class ComponentBase : IDisposable
{
    public event EventHandler? Disposed;
    
    
    public virtual void Init(IWorld world, Entity entity) { }

    public virtual void Dispose()
    {
        Disposed?.Invoke(this, EventArgs.Empty);
    }
}

public abstract class ComponentBase<TWorld> : ComponentBase where TWorld : IWorld
{
    public sealed override void Init(IWorld world, Entity entity)
    {
        Init((TWorld)world, entity);
    }

    public virtual void Init(TWorld world, Entity entity) { }
}