namespace Editor.Component;

public interface IEntityBuilder
{
    public IEntityBuilder AddComponent<TComponent>(Action<TComponent>? configure = null) where TComponent : ComponentBase, new();
    public IEntityBuilder RemoveComponent<TComponent>() where TComponent : ComponentBase;
    public IEntity Build();
}

public class EntityBuilder : IEntityBuilder
{
    private readonly List<ComponentBase> _components = [];
    
    
    internal EntityBuilder()
    {
        
    }
    
    
    public IEntityBuilder AddComponent<T>(Action<T>? configure = null) where T : ComponentBase, new()
    {
        if (_components.Any(x => x is T))
        {
            return this;
        }
        
        var component = new T();
        _components.Add(component);
        configure?.Invoke(component);
        return this;
    }

    public IEntityBuilder RemoveComponent<TComponent>() where TComponent : ComponentBase
    {
        _components.RemoveAll(x => x is TComponent);
        return this;
    }

    public IEntity Build()
    {
        return new Entity(_components);
    }
}