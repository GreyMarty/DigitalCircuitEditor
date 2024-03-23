using Editor.Component.Events;
using TinyMessenger;

namespace Editor.Component;

public interface IWorld : IDisposable
{
    public bool Initialized { get; }
    public IEnumerable<Entity> Entities { get; }
    public ITinyMessengerHub EventBus { get; }
    
    
    public void Init();
    
    public Entity Instantiate(Entity? entity = null);
    public void Destroy(Entity entity);
}

public class World : IWorld
{
    private readonly List<Entity> _entities = [];


    public bool Initialized { get; private set; }
    public IEnumerable<Entity> Entities => _entities;
    public ITinyMessengerHub EventBus { get; } = new TinyMessengerHub();

    
    public void Init()
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

    public Entity Instantiate(Entity? entity = null)
    {
        entity ??= new Entity();
        _entities.Add(entity);

        if (!Initialized)
        {
            return entity;
        }
        
        entity.Init(this);
        EventBus.Publish(new EntityInstantiated(this, entity));

        return entity;
    }
    
    public void Destroy(Entity entity)
    {
        if (!_entities.Remove(entity))
        {
            return;
        }

        entity.Dispose();
        EventBus.Publish(new EntityDestroyed(this, entity));
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}