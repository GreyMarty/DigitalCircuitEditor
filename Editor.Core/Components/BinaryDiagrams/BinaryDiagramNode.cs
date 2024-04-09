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

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(VariableId))
        {
            foreach (var (_, entity) in Nodes)
            {
                var node = entity.GetComponent<BinaryDiagramNode>()?.Component;

                if (node is null)
                {
                    continue;
                }
                
                node.VariableId = Math.Max(node.VariableId, VariableId + 1);
            }
        }
    }
}