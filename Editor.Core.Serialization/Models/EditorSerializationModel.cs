using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;

namespace Editor.Core.Serialization.Models;

internal class EditorSerializationModel
{
    public CameraSerializationModel Camera { get; set; } = default!;
    public NonTerminalNodeSerializationModel[] NonTerminalNodes { get; set; } = default!;
    public TerminalNodeSerializationModel[] TerminalNodes { get; set; } = default!;
    public ConnectionSerializationModel[] Connections { get; set; } = default!;


    public static EditorSerializationModel From(EditorContext context)
    {
        var camera = CameraSerializationModel.From(context.Camera);

        var nodes = new List<NodeSerializationModelBase>();
        var nodeIds = new Dictionary<Node, Guid>();
        
        foreach (var entity in context.Entities)
        {
            if (entity.GetComponent<ConstNode>()?.Component is { } terminalNode)
            {
                var model = TerminalNodeSerializationModel.From(terminalNode);
                nodes.Add(model);
                nodeIds.Add(terminalNode, model.Id);
            }
            else if (entity.GetComponent<BinaryDiagramNode>()?.Component is { } nonTerminalNode)
            {
                var model = NonTerminalNodeSerializationModel.From(nonTerminalNode);
                nodes.Add(model);
                nodeIds.Add(nonTerminalNode, model.Id);
            }
        }

        var connections = new List<ConnectionSerializationModel>();
        
        foreach (var entity in context.Entities)
        {
            if (entity.GetComponent<Connection>()?.Component is not { } connection)
            {
                continue;
            }

            if (ConnectionSerializationModel.From(connection, x => nodeIds[x]) is { } model)
            {
                connections.Add(model);
            }
        }

        return new EditorSerializationModel
        {
            Camera = camera,
            NonTerminalNodes = nodes.OfType<NonTerminalNodeSerializationModel>().ToArray(),
            TerminalNodes = nodes.OfType<TerminalNodeSerializationModel>().ToArray(),
            Connections = connections.ToArray()
        };
    }
}