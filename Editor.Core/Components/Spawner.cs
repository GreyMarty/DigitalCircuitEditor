using Editor.Component;

namespace Editor.Core.Components;

public class Spawner : ComponentBase<EditorWorld>
{
    private EditorWorld _world = default!;
    private Entity _entity = default!;
    
    
    public IEntityGroup? Spawnables { get; set; }
    public bool DestroyOnSpawn { get; set; } = true;


    public override void Init(EditorWorld world, Entity entity)
    {
        _world = world;
        _entity = entity;
    }

    public void Spawn()
    {
        if (Spawnables is not null)
        {
            _world.Instantiate(Spawnables);
        }

        if (DestroyOnSpawn)
        {
            _world.Destroy(_entity);
        }
    }
}