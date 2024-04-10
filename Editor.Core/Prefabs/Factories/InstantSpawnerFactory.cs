using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Prefabs.Spawners;

namespace Editor.Core.Prefabs.Factories;

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