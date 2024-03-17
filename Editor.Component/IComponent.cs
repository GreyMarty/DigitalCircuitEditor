namespace Editor.Component;

public abstract class Component
{
    public virtual void Init(IWorld world, Entity entity) { }
}