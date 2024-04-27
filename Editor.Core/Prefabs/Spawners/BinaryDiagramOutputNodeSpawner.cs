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
    
    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var result = new List<IEntity>();
        
        var root = context.Instantiate(NodeFactory.Create()
            .ConfigureComponent<Position>(p => p.Value = Position)
        );

        result.Add(root);
        
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
            
        result.Add(ghostNode);
        
        diagramNodeComponent.AddGhost(ghostNode.GetRequiredComponent<GhostNode>()!);

        return result;
    }
}