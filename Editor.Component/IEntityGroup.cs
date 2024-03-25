namespace Editor.Component;

public interface IEntityGroup
{
    public IEnumerable<Entity> GetEntities();
}

public class EntityGroup : IEntityGroup
{
    private readonly Entity[] _entities;
    
    private EntityGroup(params Entity[] entities)
    {
        _entities = entities;
    }
    
    
    public static implicit operator EntityGroup(Entity entity) => new(entity);
    
    public IEnumerable<Entity> GetEntities() => _entities;
}