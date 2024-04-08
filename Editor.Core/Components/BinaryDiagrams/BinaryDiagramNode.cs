namespace Editor.Core.Components.BinaryDiagrams;

public class BinaryDiagramNode : DiagramNode<BinaryDiagramConnectionType>
{
    public BinaryDiagramConnectionType ConnectionType { get; set; }
    
    public int VariableId { get; set; }

    public override void OnConnected(DiagramNode<BinaryDiagramConnectionType> node, Connection<BinaryDiagramConnectionType> connection)
    {
        if (node as BinaryDiagramNode is not { } binaryNode)
        {
            return;
        }

        VariableId = Math.Max(VariableId, binaryNode.VariableId + 1);
    }
}