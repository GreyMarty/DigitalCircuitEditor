using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Rendering;

namespace Editor.Core.Serialization.Models;

internal class EditorSerializationModel
{
    public CameraSerializationModel Camera { get; set; }
    public NonTerminalNodeSerializationModel[] NonTerminalNodes { get; set; } = default!;
    public TerminalNodeSerializationModel[] TerminalNodes { get; set; } = default!;
    public ConnectionSerializationModel[] Connections { get; set; } = default!;


    public static EditorSerializationModel From(IEnumerable<IEntity> entities, Camera? camera = null)
    {
        var cameraModel = camera is not null ? CameraSerializationModel.From(camera) : new CameraSerializationModel();

        var nodes = new List<NodeSerializationModelBase>();
        var nodeIds = new Dictionary<Node, Guid>();

        var entitiesList = entities.ToList();
        
        foreach (var entity in entitiesList)
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
        
        foreach (var entity in entitiesList)
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
            Camera = cameraModel,
            NonTerminalNodes = nodes.OfType<NonTerminalNodeSerializationModel>().ToArray(),
            TerminalNodes = nodes.OfType<TerminalNodeSerializationModel>().ToArray(),
            Connections = connections.ToArray()
        };
    }
}