using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Layout;
using Editor.Core.Prefabs.Factories;
using Editor.DecisionDiagrams;
using BranchNode = Editor.DecisionDiagrams.BranchNode;
using EditorBranchNode = Editor.Core.Components.Diagrams.BranchNode;

namespace Editor.Core.Prefabs.Spawners;

public class BinaryDiagramSpawner : Spawner
{
    public IEntityBuilderFactory NodeSpawnerFactory { get; set; } = new InstantSpawnerFactory<BinaryDiagramNodeSpawner>();
    public IEntityBuilderFactory ConstNodeFactory { get; set; } = new ConstNodeFactory();
    
    public BranchNode Diagram { get; set; } = default!;
    public ILayout Layout { get; set; } = new ForceDirectedLayout();
    
    
    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var nodes = new Dictionary<int, Node>();
        Spawn(Diagram, Layout.Arrange(Diagram), nodes);
        
        return nodes.Values.Select(x => x.Entity);
    }

    private Node Spawn(INode node, NodeLayoutInfo layout, Dictionary<int, Node> nodes)
    {
        if (nodes.TryGetValue(node.Id, out var nodeComponent))
        {
            return nodeComponent;
        }

        var nodeEntity = default(IEntity);
        
        if (node is TerminalNode terminalNode)
        {
            nodeEntity = Context.Instantiate(ConstNodeFactory
                .Create()
                .ConfigureComponent<Position>(x => x.Value = layout.Position(node)!.Value + Position)
                .ConfigureComponent<ConstNode>(x => x.Value = terminalNode.Value)
            );
            
            nodeComponent = nodeEntity.GetRequiredComponent<Node>()!;
            nodes[node.Id] = nodeComponent;
            return nodeComponent;
        }

        var branchNode = (BranchNode)node;

        var nodeSpawnerEntity = Context.Instantiate(NodeSpawnerFactory
            .Create()
            .ConfigureComponent<Position>(x => x.Value = layout.Position(node)!.Value + Position)
        );

        nodeEntity = nodeSpawnerEntity.GetRequiredComponent<Spawner>().Component!.Spawn().First();
        
        var branchNodeComponent = nodeEntity.GetRequiredComponent<EditorBranchNode>().Component!;
        nodeComponent = branchNodeComponent;
        nodes[node.Id] = branchNodeComponent;

        var trueNode = Spawn(branchNode.True, layout, nodes);
        var falseNode = Spawn(branchNode.False, layout, nodes);

        foreach (var (t, n, ne) in new[] { (ConnectionType.True, branchNode.True, trueNode), (ConnectionType.False, branchNode.False, falseNode) })
        {
            var connection = branchNodeComponent.Connect(t, ne)!;

            foreach (var joint in layout.Joints(branchNode, n))
            {
                var jointComponent = connection.Split(joint + Position);
                connection = jointComponent.Connection2.GetRequiredComponent<Connection>()!;
            }
        }
        
        return nodeComponent!;
    }
}