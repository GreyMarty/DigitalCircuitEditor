using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Prefabs.Spawners;

namespace Editor.Core.Behaviors;

public class SpawnOnMouseButtonDown : OnMouseButtonDownBehavior
{
    private Hoverable _hoverableComponent = default!;
    private Spawner _spawnerComponent = default!;
    
    
    protected override void OnInit()
    {
        base.OnInit();

        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
        _spawnerComponent = Entity.GetRequiredComponent<Spawner>()!;
    }

    protected override void OnMouseButtonDown(MouseButtonDown e)
    {
        if (!Entity.Active || !_hoverableComponent.Hovered)
        {
            return;
        }
        
        _spawnerComponent.Spawn();
    }
}