using Editor.Component;
using Editor.Core.Entities;

namespace Editor.Core.Components;

public class Spawner : EditorComponentBase
{
    private EditorWorld _world = default!;
    private IEntity _entity = default!;
    
    
    public IEntityTreeBuilder? Tree { get; set; }
    public bool DestroyOnSpawn { get; set; } = true;


    public override void Init(EditorWorld world, IEntity entity)
    {
        _world = world;
        _entity = entity;
    }

    public void Spawn()
    {
        if (Tree is not null)
        {
            _world.Instantiate(Tree);
        }

        if (DestroyOnSpawn)
        {
            _world.Destroy(_entity);
        }
    }
}