using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Spawners;

namespace Editor.Core.Prefabs;

public class IfDiagramNodeSpawnerFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new IfDiagramNodeSpawner
            {
                DestroyOnSpawn = true,
            });
    }
}