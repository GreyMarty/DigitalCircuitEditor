namespace Editor.Component;

public abstract class ComponentBase : IDisposable
{
    public virtual void Init(IWorld world, Entity entity) { }

    public virtual void Dispose() { }
}

public abstract class ComponentBase<TWorld> : ComponentBase where TWorld : IWorld
{
    public sealed override void Init(IWorld world, Entity entity)
    {
        Init((TWorld)world, entity);
    }

    public virtual void Init(TWorld world, Entity entity) { }
}