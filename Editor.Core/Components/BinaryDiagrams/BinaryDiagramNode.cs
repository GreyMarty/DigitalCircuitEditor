namespace Editor.Core.Components.BinaryDiagrams;

public class BinaryDiagramNode : DiagramNode<BinaryDiagramConnectionType>
{
    public BinaryDiagramConnectionType ConnectionType { get; set; }
    
    public int VariableId { get; set; }
}