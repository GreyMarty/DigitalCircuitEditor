using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.IfDiagrams;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Factories.IfDiagrams;

namespace Editor.Core.Prefabs.Spawners;

public class IfDiagramNodeSpawner : Spawner
{
    public IEntityBuilderFactory NodeFactory { get; set; } = new IfDiagramNodeFactory();
    public IEntityBuilderFactory GhostConnectionFactory { get; set; } = new GhostConnectionFactory<IfDiagramConnection>();
    public IEntityBuilderFactory GhostNodeFactory { get; set; } = new IfDiagramGhostNodeFactory();
    

    protected override void OnSpawn(EditorContext context)
    {
        var root = context.Instantiate(NodeFactory.Create()
            .ConfigureComponent<Position>(p => p.Value = Position)
        );
        
        for (var i = 0; i < 3; i++)
        {
            var offset = new Vector2(-8 + i * 8, 8);
            
            var type = i switch
            {
                0 => IfDiagramConnectionType.If,
                1 => IfDiagramConnectionType.True,
                2 => IfDiagramConnectionType.False,
            };
            
            var ghostNode = context.Instantiate(GhostNodeFactory.Create()
                .ConfigureComponent<Position>(p => p.Value = offset)
                .ConfigureComponent<GhostNode<IfDiagramConnectionType>>(x => x.ConnectionType = type)
                .AddComponent(new ChildOf
                {
                    Parent = root
                })
            );
            
            context.Instantiate(GhostConnectionFactory.Create()
                .ConfigureComponent<ChildOf>(x => x.Parent = root)
                .ConfigureComponent<IfDiagramConnection>(x =>
                {
                    x.Target = ghostNode;
                    x.Type = type;
                })
            );
        }
    }
}