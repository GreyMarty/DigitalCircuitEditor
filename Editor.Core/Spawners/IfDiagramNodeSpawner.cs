using System.Numerics;
using Editor.Component;
using Editor.Core.Prefabs;
using Editor.Core.Components;

namespace Editor.Core.Spawners;

public class IfDiagramNodeSpawner : Spawner
{
    private ChildOf? _childOfComponent = default!;


    public IfDiagramConnectionType ConnectionType { get; set; } = IfDiagramConnectionType.Direct;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        base.OnInit(context, entity);

        _childOfComponent = entity.GetComponent<ChildOf>()?.Component;
    }

    protected override void OnSpawn(EditorContext context)
    {
        var root = context.Instantiate(IfDiagramNodePrefab.CreateBuilder(Position));

        if (_childOfComponent?.Parent is not null)
        {
            context.Instantiate(IfDiagramConnectionPrefab.CreateBuilder(_childOfComponent.Parent, root, ConnectionType));
        }
        
        for (var i = 0; i < 3; i++)
        {
            var offset = new Vector2(-8 + i * 8, 8);
            
            var type = i switch
            {
                0 => IfDiagramConnectionType.If,
                1 => IfDiagramConnectionType.True,
                2 => IfDiagramConnectionType.False,
            };
            
            var spawner = context.Instantiate(IfDiagramNodeSpawnerPrefab.CreateBuilder(offset, type)
                .AddComponent(new ChildOf
                {
                    Parent = root
                })
            );
            
            context.Instantiate(IfDiagramGhostConnectionPrefab.CreateBuilder(root, spawner, type));
        }
    }
}