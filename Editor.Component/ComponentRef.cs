namespace Editor.Component;

public class ComponentRef<T> where T : ComponentBase
{
    private Entity _entity;
    private T? _component;
    
    
    internal ComponentRef(Entity entity, T component)
    {
        _entity = entity;
        _component = component;
    }


    public T? Component => _entity.Alive ? _component : null;


    public static implicit operator T?(ComponentRef<T> component) => component.Component;
}