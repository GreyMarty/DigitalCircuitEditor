using Editor.Component.Exceptions;

namespace Editor.Component;

public sealed class Entity : IDisposable
{
    private readonly List<ComponentBase> _components = [];
    
    
    public bool Alive { get; internal set; }
    public bool Initialized { get; private set; }
    
    public IEnumerable<ComponentBase> Components => _components;

    
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

    public void Dispose()
    {
        _components.ForEach(x => x.Dispose());
        Initialized = false;
        Alive = false;
    }

    public T? GetComponent<T>() where T : ComponentBase
    {
        return _components.FirstOrDefault(x => x is T) as T;
    }

    public T GetRequiredComponent<T>() where T : ComponentBase
    {
        return GetComponent<T>() ?? throw new ComponentRequiredException(typeof(T));
    }
    
    public T AddComponent<T>(Action<T>? configure = null) where T : ComponentBase, new()
    {
        var existingComponent = GetComponent<T>();
        
        if (existingComponent is not null)
        {
            return existingComponent;
        }
        
        var component = new T();
        _components.Add(component);
        configure?.Invoke(component);
        return component;
    }

    public bool RemoveComponent<T>() where T : ComponentBase
    {
        var component = _components.FirstOrDefault(x => x is T);
        return component != null && _components.Remove(component);
    }
}