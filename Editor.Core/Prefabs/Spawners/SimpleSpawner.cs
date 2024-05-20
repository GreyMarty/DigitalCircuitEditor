using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Prefabs.Spawners;

public class SimpleSpawner<TFactory> : Spawner where TFactory : IEntityBuilderFactory, new()
{
    public SimpleSpawner()
    {
    }

    public SimpleSpawner(TFactory factory)
    {
        Factory = factory;
    }
    
    public IEntityBuilderFactory Factory { get; set; } = new TFactory();
    
    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        return [ context.Instantiate(Factory.Create()
            .ConfigureComponent<Position>(x => x.Value = Position)
        ) ];
    }
}