using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Spawners;

namespace Editor.Core.Components.Behaviors;

public class SpawnOnMouseButtonDownBehavior : OnMouseButtonDownBehavior
{
    private Spawner _spawnerComponent = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        base.OnInit(context, entity);
        
        _spawnerComponent = entity.GetRequiredComponent<Spawner>()!;
    }

    protected override void OnClick(MouseButtonDown e, bool hovered)
    {
        if (hovered && e.Button == MouseButton.Left)
        {
            _spawnerComponent.Spawn();
        }
    }
}