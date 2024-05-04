using GraphX.Common.Models;

namespace Editor.DecisionDiagrams.Layout;

public class LayoutVertex(int nodeId) : VertexBase
{
    public int NodeId { get; } = nodeId;
}