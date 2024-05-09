using System.Numerics;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;

namespace Editor.Core.Serialization.Models;

internal class ConnectionSerializationModel
{
    public Guid SourceId { get; set; }
    public Guid TargetId { get; set; }
    public ConnectionType Type { get; set; }

    public Vector2[] Joints { get; set; } = default!;


    public static ConnectionSerializationModel? From(Connection connection, Func<Node, Guid> getNodeId)
    {
        var childOfComponent = connection.Entity.GetRequiredComponent<ChildOf>().Component!;
        var sourceNode = childOfComponent.Parent!.GetComponent<Node>()?.Component;

        if (sourceNode is null)
        {
            return null;
        }

        var sourceId = getNodeId(sourceNode);

        var type = connection.Type;
        
        var joints = new List<Vector2>();
        var targetNode = connection.Target?.GetComponent<Node>()?.Component;

        while (targetNode is null)
        {
            var targetJoint = connection.Target?.GetComponent<ConnectionJoint>()?.Component;

            if (targetJoint is null)
            {
                return null;
            }
            
            joints.Add(targetJoint.Entity.GetRequiredComponent<Position>().Component!.Value);

            connection = targetJoint.Connection2.GetRequiredComponent<Connection>().Component!;
            targetNode = connection.Target?.GetComponent<Node>()?.Component;
        }
        
        var targetId = getNodeId(targetNode);
        
        return new ConnectionSerializationModel
        {
            SourceId = sourceId,
            TargetId = targetId,
            Type = type,
            Joints = joints.ToArray()
        };
    }
}