using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Prefabs.Spawners;

namespace Editor.Core.Behaviors;

public class SpawnBehavior : BehaviorBase<EditorContext>
{
    private Spawner _spawnerComponent = default!;
    
    
    protected override void OnInit()
    {
        base.OnInit();
        
        _spawnerComponent = Entity.GetRequiredComponent<Spawner>()!;
    }

    protected override void Perform()
    {
        _spawnerComponent.Spawn();
    }
}