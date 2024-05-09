using Editor.Core.Components;
using Editor.Core.Components.Diagrams.BinaryDiagrams;

namespace Editor.Core.Serialization.Models;

internal class NonTerminalNodeSerializationModel : NodeSerializationModelBase
{
    public int VariableId { get; set; }


    public static NonTerminalNodeSerializationModel From(BinaryDiagramNode node) => new()
    {
        Id = Guid.NewGuid(),
        VariableId = node.VariableId,
        Position = node.Entity.GetRequiredComponent<Position>().Component!.Value
    };
}