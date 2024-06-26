﻿using System.Numerics;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Prefabs.Spawners;

public abstract class Spawner : EditorComponentBase
{
    private Position _positionComponent = default!;
    
    public bool DestroyOnSpawn { get; set; }

    protected Vector2 Position => _positionComponent.Value;
    
    
    public IEnumerable<IEntity> Spawn()
    {
        if (!Entity.Alive)
        {
            return Enumerable.Empty<IEntity>();
        }
        
        var result = OnSpawn(Context);

        if (DestroyOnSpawn)
        {
            Context.Destroy(Entity);
        }

        return result;
    }
    
    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
    }

    protected abstract IEnumerable<IEntity> OnSpawn(EditorContext context);
}