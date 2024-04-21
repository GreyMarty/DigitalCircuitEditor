using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.DecisionDiagrams;
using BranchNode = Editor.DecisionDiagrams.BranchNode;

namespace Editor.Core.Converters;

public static class EntitiesToDiagramConverter
{
    public static DecisionDiagram Convert(OutputNode nodeComponent)
    {
        return new DecisionDiagram(
            nodeComponent.OutputId,
            Convert(nodeComponent.Nodes[ConnectionType.Direct].GetRequiredComponent<Node>()!)
        );
    }
    
    public static INode Convert(Node nodeComponent)
    {
        switch (nodeComponent)
        {
            case ConstNode constNodeComponent:
                return new TerminalNode(constNodeComponent.Value);
            
            case BinaryDiagramNode binaryNodeComponent:
            {
                var trueNodeComponent = binaryNodeComponent.Nodes[ConnectionType.True].GetRequiredComponent<Node>();
                var falseNodeComponent = binaryNodeComponent.Nodes[ConnectionType.False].GetRequiredComponent<Node>();
                return new BranchNode(
                    binaryNodeComponent.VariableId,
                    Convert(trueNodeComponent!),
                    Convert(falseNodeComponent!)
                );
            }
            
            default:
                throw new InvalidOperationException("Could not convert nodes.");
        }
    }
}