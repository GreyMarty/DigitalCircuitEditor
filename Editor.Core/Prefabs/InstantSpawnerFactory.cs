using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Spawners;

namespace Editor.Core.Prefabs;

public class InstantSpawnerFactory<TSpawner> : IEntityBuilderFactory
    where TSpawner : Spawner, new()
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new TSpawner
            {
                DestroyOnSpawn = true,
            });
    }
}