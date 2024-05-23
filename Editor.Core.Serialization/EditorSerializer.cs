using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering;
using Editor.Core.Serialization.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Vector = GraphX.Measure.Vector;

namespace Editor.Core.Serialization;

public class EditorSerializer
{
    public IEntityBuilderFactory NodeSpawnerFactory { get; set; } = new InstantSpawnerFactory<BinaryDiagramNodeSpawner>();
    public IEntityBuilderFactory ConstNodeFactory { get; set; } = new ConstNodeFactory();


    public void Serialize(TextWriter writer, IEnumerable<IEntity> entities, Camera? camera = null)
    {
        var model = EditorSerializationModel.From(entities, camera);

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        serializer.Serialize(writer, model);
    }
    
    public void Serialize(TextWriter writer, EditorContext context)
    {
        Serialize(writer, context.Entities, context.Camera);
    }

    public void Deserialize(TextReader reader, EditorContext context, bool deleteEntities = false, bool selectOnCreate = false, bool deserializeCamera = false, Vector2 offset = default)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var model = deserializer.Deserialize<EditorSerializationModel>(reader);
        var nodes = new Dictionary<Guid, Node>();

        if (deserializeCamera)
        {
            context.Camera.Position = model.Camera.Position;
            context.Camera.Scale = model.Camera.Scale;
        }

        if (deleteEntities)
        {
            foreach (var entity in context.Entities.ToList())
            {
                if (entity.GetComponent<Node>() is not null || entity.GetComponent<Connection>() is not null)
                {
                    context.Destroy(entity);
                }
            }
        }

        foreach (var node in model.TerminalNodes)
        {
            var entity = context.Instantiate(ConstNodeFactory
                .Create()
                .ConfigureComponent<Position>(x => x.Value = node.Position + offset)
                .ConfigureComponent<ConstNode>(x => x.Value = node.Value)
            );

            entity.GetRequiredComponent<Selectable>().Component!.Selected = selectOnCreate;
            nodes[node.Id] = entity.GetRequiredComponent<Node>()!;
        }

        foreach (var node in model.NonTerminalNodes)
        {
            var spawnerEntity = context.Instantiate(NodeSpawnerFactory
                .Create()
                .ConfigureComponent<Position>(x => x.Value = node.Position + offset)
            );

            var entity = spawnerEntity.GetRequiredComponent<Spawner>().Component!.Spawn().First();
            var branchNodeComponent = entity.GetRequiredComponent<BinaryDiagramNode>().Component!;
            entity.GetRequiredComponent<Selectable>().Component!.Selected = selectOnCreate;
            branchNodeComponent.VariableId = node.VariableId;
            nodes[node.Id] = branchNodeComponent;
        }

        foreach (var connection in model.Connections)
        {
            var sourceNode = (BranchNode)nodes[connection.SourceId];
            var targetNode = nodes[connection.TargetId];

            var connectionComponent = sourceNode.Connect(connection.Type, targetNode)!;
            connectionComponent.Entity.GetRequiredComponent<Selectable>().Component!.Selected = selectOnCreate;

            foreach (var joint in connection.Joints)
            {
                var jointComponent = connectionComponent.Split(joint + offset);
                connectionComponent = jointComponent.Connection2.GetRequiredComponent<Connection>()!;

                jointComponent.Entity.GetRequiredComponent<Selectable>().Component!.Selected = selectOnCreate;
                jointComponent.Connection2.GetRequiredComponent<Selectable>().Component!.Selected = selectOnCreate;
            }
        }
    }
}