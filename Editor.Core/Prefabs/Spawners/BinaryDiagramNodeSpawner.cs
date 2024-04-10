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
    public IEntityBuilderFactory GhostConnectionFactory { get; set; } = new GhostConnectionFactory<BinaryDiagramConnection>();
    public IEntityBuilderFactory GhostNodeFactory { get; set; } = new BinaryDiagramGhostNodeFactory();
    

    protected override void OnSpawn(EditorContext context)
    {
        var root = context.Instantiate(NodeFactory.Create()
            .ConfigureComponent<Position>(p => p.Value = Position)
        );

        var diagramNodeComponent = root.GetRequiredComponent<BinaryBranchNode>().Component!;
        
        for (var i = 0; i < 2; i++)
        {
            var offset = new Vector2(-6 + i * 12, 6);
            
            var type = i switch
            {
                0 => BinaryDiagramConnectionType.True,
                1 => BinaryDiagramConnectionType.False,
            };
            
            var ghostNode = context.Instantiate(GhostNodeFactory.Create()
                .ConfigureComponent<Position>(p => p.Value = offset)
                .ConfigureComponent<GhostNode<BinaryDiagramConnectionType>>(x =>
                {
                    x.ConnectionType = type;
                })
                .AddComponent(new ChildOf
                {
                    Parent = root,
                    DestroyWithParent = true
                })
            );
            
            var ghostConnection = context.Instantiate(GhostConnectionFactory.Create()
                .ConfigureComponent<ChildOf>(x =>
                {
                    x.Parent = root;
                })
                .ConfigureComponent<BinaryDiagramConnection>(x =>
                {
                    x.Target = ghostNode;
                    x.Type = type;
                })
            );

            diagramNodeComponent.GhostNodes[type] = ghostNode;
            diagramNodeComponent.GhostConnections[type] = ghostConnection;
        }
    }
}