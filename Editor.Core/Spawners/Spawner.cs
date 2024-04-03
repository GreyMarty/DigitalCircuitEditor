using System.Numerics;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Spawners;

public abstract class Spawner : EditorComponentBase
{
    private Position _positionComponent = default!;
    
    public bool DestroyOnSpawn { get; set; }

    protected Vector2 Position => _positionComponent.Value;
    
    
    public void Spawn()
    {
        if (!Entity.Alive)
        {
            return;
        }
        
        OnSpawn(Context);

        if (DestroyOnSpawn)
        {
            Context.Destroy(Entity);
        }
    }
    
    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
    }

    protected abstract void OnSpawn(EditorContext context);
}