using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Factories.BinaryDiagrams;

namespace Editor.Core.Prefabs.Spawners;

public class BinaryDiagramNodeSpawner : Spawner
{
    public IEntityBuilderFactory NodeFactory { get; set; } = new BinaryDiagramNodeFactory();
    public IEntityBuilderFactory GhostConnectionFactory { get; set; } = new GhostConnectionFactory();
    public IEntityBuilderFactory GhostNodeFactory { get; set; } = new GhostNodeFactory();
    

    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var result = new List<IEntity>();
        
        var root = context.Instantiate(NodeFactory.Create()
            .ConfigureComponent<Position>(p => p.Value = Position)
        );

        result.Add(root);
        
        var diagramNodeComponent = root.GetRequiredComponent<BinaryDiagramNode>().Component!;
        
        for (var i = 0; i < 2; i++)
        {
            var offset = new Vector2(-6 + i * 12, 6);
            
            var type = i switch
            {
                0 => ConnectionType.False,
                1 => ConnectionType.True,
            };
            
            var ghostNode = context.Instantiate(GhostNodeFactory.Create()
                .ConfigureComponent<Position>(p => p.Value = offset)
                .ConfigureComponent<GhostNode>(x =>
                {
                    x.ConnectionType = type;
                })
                .AddComponent(new ChildOf
                {
                    Parent = root,
                    DestroyWithParent = true
                })
            );
            
            result.Add(ghostNode);
            
            diagramNodeComponent.AddGhost(ghostNode.GetRequiredComponent<GhostNode>()!);
        }

        return result;
    }
}