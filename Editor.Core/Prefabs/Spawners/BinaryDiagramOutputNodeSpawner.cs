using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Factories.BinaryDiagrams;

namespace Editor.Core.Prefabs.Spawners;

public class BinaryDiagramOutputNodeSpawner : Spawner 
{
    public IEntityBuilderFactory NodeFactory { get; set; } = new OutputNodeFactory();
    public IEntityBuilderFactory GhostConnectionFactory { get; set; } = new GhostConnectionFactory();
    public IEntityBuilderFactory GhostNodeFactory { get; set; } = new GhostNodeFactory();
    
    protected override void OnSpawn(EditorContext context)
    {
        var root = context.Instantiate(NodeFactory.Create()
            .ConfigureComponent<Position>(p => p.Value = Position)
        );
        
        var diagramNodeComponent = root.GetRequiredComponent<BranchNode>().Component!;

        var type = ConnectionType.Direct;
        
        var ghostNode = context.Instantiate(GhostNodeFactory.Create()
            .ConfigureComponent<Position>(p => p.Value = new Vector2(0, 6))
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
            
        var ghostConnection = context.Instantiate(GhostConnectionFactory.Create()
            .ConfigureComponent<ChildOf>(x =>
            {
                x.Parent = root;
            })
            .ConfigureComponent<Connection>(x =>
            {
                x.Target = ghostNode;
                x.Type = type;
            })
        );

        diagramNodeComponent.GhostNodes[type] = ghostNode;
        diagramNodeComponent.GhostConnections[type] = ghostConnection;
    }
}