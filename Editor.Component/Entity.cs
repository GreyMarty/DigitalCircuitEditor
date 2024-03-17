namespace Editor.Component;

public sealed class Entity
{
    private readonly List<Component> _components = [];
    
    
    public bool Alive { get; internal set; }
    public bool Initialized { get; private set; }
    
    public IEnumerable<Component> Components => _components;

    
    public void Init(IWorld world)
    {
        if (Initialized)
        {
            return;
        }
        
        _components.ForEach(x => x.Init(world, this));
        Initialized = true;
        Alive = true;
    }

    public T? GetComponent<T>() where T : Component
    {
        return _components.FirstOrDefault(x => x is T) as T;
    }
    
    public T AddComponent<T>() where T : Component, new()
    {
        var existingComponent = GetComponent<T>();
        
        if (existingComponent is not null)
        {
            return existingComponent;
        }
        
        var component = new T();
        _components.Add(component);
        return component;
    }

    public bool RemoveComponent<T>() where T : Component
    {
        var component = _components.FirstOrDefault(x => x is T);
        return component != null && _components.Remove(component);
    }
}