namespace Editor.Core.Components.Diagrams.BinaryDiagrams;

public class BinaryDiagramNode : BranchNode<BinaryDiagramConnectionType>
{
    public BinaryDiagramConnectionType ConnectionType { get; set; }

    public int MinVariableId { get; set; } = 0;
    public int VariableId { get; set; }

    public override string? Label
    {
        get => $"x{VariableId}";
        set { }
    }

    public override void OnConnected(BranchNode<BinaryDiagramConnectionType> parent, Connection<BinaryDiagramConnectionType> connection)
    {
        if (parent as BinaryDiagramNode is not { } binaryNode)
        {
            return;
        }

        MinVariableId = binaryNode.VariableId + 1;
        VariableId = Math.Max(VariableId, MinVariableId);
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

                node.MinVariableId = VariableId + 1;
                node.VariableId = Math.Max(node.VariableId, node.MinVariableId);
            }
        }
    }
}