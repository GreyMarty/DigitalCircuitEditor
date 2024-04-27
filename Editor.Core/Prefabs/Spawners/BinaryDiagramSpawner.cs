using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Layout;
using Editor.Core.Prefabs.Factories;
using Editor.DecisionDiagrams;
using BranchNode = Editor.DecisionDiagrams.BranchNode;

namespace Editor.Core.Prefabs.Spawners;

public class BinaryDiagramSpawner : Spawner
{
    public IEntityBuilderFactory NodeSpawnerFactory { get; set; } = new InstantSpawnerFactory<BinaryDiagramNodeSpawner>();
    public IEntityBuilderFactory ConstNodeFactory { get; set; } = new ConstNodeFactory();
    public IEntityBuilderFactory ConnectionFactory { get; set; } = new ConnectionFactory();
    public IEntityBuilderFactory JointFactory { get; set; } = new ConnectionJointFactory();
    public BranchNode Diagram { get; set; } = default!;
    public ILayout Layout { get; set; } = new ForceDirectedLayout();
    
    
    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var result = new List<IEntity>();

        
        
        return result;
    }

    private IEntity Spawn(INode node, NodeLayoutInfo layout, Dictionary<INode, IEntity> nodes)
    {
        if (nodes.TryGetValue(node, out var nodeEntity))
        {
            return nodeEntity;
        }
        
        if (node is TerminalNode terminalNode)
        {
            nodeEntity = Context.Instantiate(ConstNodeFactory
                .Create()
                .ConfigureComponent<ConstNode>(x => x.Value = terminalNode.Value)
            );
            nodes[node] = nodeEntity;
        }

        var branchNode = (BranchNode)node;

        nodeEntity = Context.Instantiate(NodeSpawnerFactory
            .Create()
            .ConfigureComponent<Position>(x => x.Value = layout.Position(node)!.Value)
            .ConfigureComponent<BinaryDiagramNode>(x => x.VariableId = branchNode.VariableId)
        );
        nodes[node] = nodeEntity;

        var trueNode = Spawn(branchNode.True, layout, nodes);
        var falseNode = Spawn(branchNode.False, layout, nodes);

        var firstJoint = default(IEntity);
        var lastConnection = default(Connection);
        
        foreach (var jointPosition in layout.Joints(node, branchNode.True))
        {
            var jointEntity = Context.Instantiate(JointFactory.Create()
                .ConfigureComponent<Position>(x =>
                {
                    x.Value = jointPosition;
                })
            );
            firstJoint ??= jointEntity;

            if (lastConnection is not null)
            {
                lastConnection.Target = jointEntity;
            }
            
            var jointComponent = jointEntity.GetRequiredComponent<ConnectionJoint>().Component!;
            
            var connectionEntity = Context.Instantiate(ConnectionFactory.Create()
                .ConfigureComponent<ChildOf>(x =>
                {
                    x.Parent = jointEntity;
                })
            );

            jointComponent.Connection1 = lastConnection?.Entity;
            lastConnection = connectionEntity.GetRequiredComponent<Connection>();
            jointComponent.Connection2 = lastConnection!.Entity;
        }

        lastConnection.Target = trueNode;

        // var connection = Context.Instantiate(ConnectionFactory.Create()
        //     .ConfigureComponent<ChildOf>(x => x.Parent = childOfComponent.Parent)
        //     .ConfigureComponent<Connection>(x =>
        //     {
        //         x.Target = Entity;
        //         x.Type = ghostNodeComponent.ConnectionType;
        //     })
        // );
        //
        // parentNode.Connections[connectionType] = connection;
        // parentNode.Nodes[connectionType] = Entity;
        //     
        // Entity.GetRequiredComponent<BranchNode>().Component?.OnConnected(parentNode, connection.GetRequiredComponent<Connection>()!);

        return null;
    }
}