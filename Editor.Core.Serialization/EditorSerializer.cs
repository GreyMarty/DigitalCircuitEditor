using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Serialization.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Editor.Core.Serialization;

public class EditorSerializer
{
    public IEntityBuilderFactory NodeSpawnerFactory { get; set; } = new InstantSpawnerFactory<BinaryDiagramNodeSpawner>();
    public IEntityBuilderFactory ConstNodeFactory { get; set; } = new ConstNodeFactory();
    
    
    public void Serialize(EditorContext context, TextWriter writer)
    {
        var model = EditorSerializationModel.From(context);

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        serializer.Serialize(writer, model);
    }

    public void Deserialize(EditorContext context, TextReader reader)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var model = deserializer.Deserialize<EditorSerializationModel>(reader);
        var nodes = new Dictionary<Guid, Node>();

        context.Camera.Position = model.Camera.Position;
        context.Camera.Scale = model.Camera.Scale;
        
        foreach (var entity in context.Entities.ToList())
        {
            if (entity.GetComponent<Node>() is not null || entity.GetComponent<Connection>() is not null)
            {
                context.Destroy(entity);
            }
        }
        
        foreach (var node in model.TerminalNodes)
        {
            var entity = context.Instantiate(ConstNodeFactory
                .Create()
                .ConfigureComponent<Position>(x => x.Value = node.Position)
                .ConfigureComponent<ConstNode>(x => x.Value = node.Value)
            );
            
            nodes[node.Id] = entity.GetRequiredComponent<Node>()!;
        }

        foreach (var node in model.NonTerminalNodes)
        {
            var spawnerEntity = context.Instantiate(NodeSpawnerFactory
                .Create()
                .ConfigureComponent<Position>(x => x.Value = node.Position)
            );

            var entity = spawnerEntity.GetRequiredComponent<Spawner>().Component!.Spawn().First();
            var branchNodeComponent = entity.GetRequiredComponent<BinaryDiagramNode>().Component!;
            branchNodeComponent.VariableId = node.VariableId;
            nodes[node.Id] = branchNodeComponent;
        }

        foreach (var connection in model.Connections)
        {
            var sourceNode = (BranchNode)nodes[connection.SourceId];
            var targetNode = nodes[connection.TargetId];

            var connectionComponent = sourceNode.Connect(connection.Type, targetNode)!;
            
            foreach (var joint in connection.Joints)
            {
                var jointComponent = connectionComponent.Split(joint);
                connectionComponent = jointComponent.Connection2.GetRequiredComponent<Connection>()!;
            }
        }
    }
}