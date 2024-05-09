using Editor.Core.Components;
using Editor.Core.Components.Diagrams;

namespace Editor.Core.Serialization.Models;

internal class TerminalNodeSerializationModel : NodeSerializationModelBase
{
    public bool Value { get; set; }


    public static TerminalNodeSerializationModel From(ConstNode node) => new()
    {
        Id = Guid.NewGuid(),
        Value = node.Value,
        Position = node.Entity.GetRequiredComponent<Position>().Component!.Value
    };
}