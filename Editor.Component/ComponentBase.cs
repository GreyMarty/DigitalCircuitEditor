using TinyMessenger;

namespace Editor.Component;

public abstract class ComponentBase
{
    public bool Initialized { get; private set; }


    internal void Init(IWorld world, IEntity entity)
    {
        if (Initialized)
        {
            return;
        }
        
        OnInit(world, entity);
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
    
    protected virtual void OnInit(IWorld world, IEntity entity) { }
    protected virtual void OnDestroy() { }
}

public abstract class ComponentBase<TWorld> : ComponentBase 
    where TWorld : IWorld
{
    protected sealed override void OnInit(IWorld world, IEntity entity)
    {
        OnInit((TWorld)world, entity);
    }

    protected virtual void OnInit(TWorld context, IEntity entity) { }
}