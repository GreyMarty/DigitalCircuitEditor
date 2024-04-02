using System.Numerics;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Spawners;

public abstract class Spawner : EditorComponentBase
{
    private EditorContext _context = default!;
    private IEntity _entity = default!;
    
    private Position _positionComponent = default!;
    
    public bool DestroyOnSpawn { get; set; }

    protected Vector2 Position => _positionComponent.Value;
    
    
    public void Spawn()
    {
        if (!_entity.Alive)
        {
            return;
        }
        
        OnSpawn(_context);

        if (DestroyOnSpawn)
        {
            _context.Destroy(_entity);
        }
    }
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _context = context;
        _entity = entity;

        _positionComponent = entity.GetRequiredComponent<Position>()!;
    }

    protected abstract void OnSpawn(EditorContext context);
}