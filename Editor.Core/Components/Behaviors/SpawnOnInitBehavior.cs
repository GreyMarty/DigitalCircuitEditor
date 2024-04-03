using Editor.Component;
using Editor.Core.Spawners;

namespace Editor.Core.Components.Behaviors;

public class SpawnOnInitBehavior : EditorComponentBase
{
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        base.OnInit(context, entity);
        
        var spawnerComponent = entity.GetRequiredComponent<Spawner>()!;
        spawnerComponent.Component!.Spawn();
    }
}