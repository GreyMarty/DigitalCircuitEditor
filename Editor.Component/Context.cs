using Editor.Component.Events;
using TinyMessenger;

namespace Editor.Component;

public interface IContext : IDisposable
{
    public bool Initialized { get; }
    public IEnumerable<IEntity> Entities { get; }
    public IEventBus EventBus { get; }
    
    
    public void Init();
    
    public IEntity Instantiate(IEntityBuilder? builder = null);
    public IEnumerable<IEntity> Instantiate(IEntityTreeBuilder treeBuilder);
    public void Destroy(IEntity entity);
}

public class Context : IContext
{
    private readonly List<IEntity> _entities = [];


    public bool Initialized { get; private set; }
    public IEnumerable<IEntity> Entities => _entities;
    public IEventBus EventBus { get; } = new EventBus(new TinyMessengerHub());

    
    public virtual void Init()
    {
        if (Initialized)
        {
            return;
        }

        foreach (var entity in Entities)
        {
            entity.Init(this);
            EventBus.Publish(new EntityInstantiated(this, entity));
        }
        
        Initialized = true;
    }

    public IEntity Instantiate(IEntityBuilder? builder = null)
    {
        var entity = builder?.Build() ?? new Entity();
        _entities.Add(entity);

        if (!Initialized)
        {
            return entity;
        }
        
        entity.Init(this);
        EventBus.Publish(new EntityInstantiated(this, entity));

        return entity;
    }

    public IEnumerable<IEntity> Instantiate(IEntityTreeBuilder treeBuilder)
    {
        var entities = new List<IEntity>();
        
        foreach (var entity in treeBuilder.Build())
        {
            entities.Add(entity);
            _entities.Add(entity);

            if (!Initialized)
            {
                continue;
            }
            
            entity.Init(this);
            EventBus.Publish(new EntityInstantiated(this, entity));
        }

        return entities;
    }

    public void Destroy(IEntity entity)
    {
        EventBus.Publish(new EntityDestroying(this, entity));
        
        if (!_entities.Remove(entity))
        {
            return;
        }

        EventBus.Publish(new EntityDestroyed(this, entity));
        entity.Destroy();
    }
    
    public virtual void Dispose()
    {
        var entities = _entities.ToArray();
        
        foreach (var entity in entities)
        {
            Destroy(entity);
        }

        Initialized = false;
    }
}