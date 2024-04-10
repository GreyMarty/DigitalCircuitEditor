using Editor.Core.Components;
using Editor.Core.Prefabs.Spawners;

namespace Editor.Core.Behaviors;

public class SpawnOnInit : EditorComponentBase
{
    protected override void OnInit()
    {
        var spawnerComponent = Entity.GetRequiredComponent<Spawner>().Component!;
        spawnerComponent.Spawn();
    }
}