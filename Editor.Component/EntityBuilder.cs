namespace Editor.Component;

public interface IEntityBuilder
{
    public IEntityBuilder AddComponent<T>(T? component = null) where T : ComponentBase, new();
    public IEntityBuilder RemoveComponent<T>() where T : ComponentBase;
    public IEntityBuilder ConfigureComponent<T>(Action<T> configure) where T : ComponentBase;
    public IEntity Build();
}

public class EntityBuilder : IEntityBuilder
{
    private readonly List<ComponentBase> _components = [];
    
    
    internal EntityBuilder()
    {
        
    }
    
    
    public IEntityBuilder AddComponent<T>(T? component = null) where T : ComponentBase, new()
    {
        if (_components.Any(x => x is T))
        {
            return this;
        }
        
        component ??= new T();
        _components.Add(component);
        return this;
    }

    public IEntityBuilder RemoveComponent<TComponent>() where TComponent : ComponentBase
    {
        _components.RemoveAll(x => x is TComponent);
        return this;
    }

    public IEntityBuilder ConfigureComponent<T>(Action<T> configure) where T : ComponentBase
    {
        var component = (T)_components.Find(x => x is T)!;
        configure(component);
        return this;
    }

    public IEntity Build()
    {
        return new Entity(_components);
    }
}