using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.DecisionDiagrams;
using BranchNode = Editor.DecisionDiagrams.BranchNode;

namespace Editor.Core.Converters;

public static class EntitiesToDiagramConverter
{
    public static DecisionDiagram Convert(OutputNode nodeComponent)
    {
        var id = 0;
        
        return new DecisionDiagram(
            nodeComponent.OutputId,
            Convert(nodeComponent.Nodes[ConnectionType.Direct], [], ref id)
        );
    }
    
    public static DecisionDiagram Convert(Node nodeComponent)
    {
        var id = 0;
        
        return new DecisionDiagram(
            0,
            Convert(nodeComponent, [], ref id)
        );
    }
    
    private static INode Convert(Node nodeComponent, Dictionary<Node, INode> nodes, ref int id)
    {
        if (nodes.TryGetValue(nodeComponent, out var cachedNode))
        {
            return cachedNode;
        }

        var node = default(INode);
        
        switch (nodeComponent)
        {
            case ConstNode constNodeComponent:
                node = new TerminalNode(id++, constNodeComponent.Value);
                break;
            
            case BinaryDiagramNode binaryNodeComponent:
            {
                var trueNodeComponent = binaryNodeComponent.Nodes[ConnectionType.True];
                var falseNodeComponent = binaryNodeComponent.Nodes[ConnectionType.False];
                node = new BranchNode(
                    id++,
                    binaryNodeComponent.VariableId,
                    Convert(trueNodeComponent!, nodes, ref id),
                    Convert(falseNodeComponent!, nodes, ref id)
                );
                break;
            }
            
            default:
                throw new InvalidOperationException("Could not convert nodes.");
        }

        nodes[nodeComponent] = node;
        return node;
    }
}