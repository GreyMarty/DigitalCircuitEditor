namespace Editor.Core.Components.Diagrams.BinaryDiagrams;

public class BinaryBranchNode : BranchNode<BinaryDiagramConnectionType>
{
    public BinaryDiagramConnectionType ConnectionType { get; set; }
    
    public int VariableId { get; set; }

    public override string? Label
    {
        get => $"x{VariableId}";
        set { }
    }

    public override void OnConnected(BranchNode<BinaryDiagramConnectionType> parent, Connection<BinaryDiagramConnectionType> connection)
    {
        if (parent as BinaryBranchNode is not { } binaryNode)
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
                var node = entity.GetComponent<BinaryBranchNode>()?.Component;

                if (node is null)
                {
                    continue;
                }
                
                node.VariableId = Math.Max(node.VariableId, VariableId + 1);
            }
        }
    }
}