using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Circuits;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Factories.Circuits;
using Editor.DecisionDiagrams.Circuits;
using Editor.DecisionDiagrams.Circuits.Gates;
using Editor.DecisionDiagrams.Layout;
using GraphX.Logic.Algorithms.LayoutAlgorithms;
using AndGate = Editor.DecisionDiagrams.Circuits.Gates.AndGate;
using Constant = Editor.DecisionDiagrams.Circuits.Constant;
using MuxGate = Editor.DecisionDiagrams.Circuits.Gates.MuxGate;
using NotGate = Editor.DecisionDiagrams.Circuits.Gates.NotGate;
using OrGate = Editor.DecisionDiagrams.Circuits.Gates.OrGate;

namespace Editor.Core.Prefabs.Spawners.Circuits;

public class CircuitSpawner : Spawner
{
    public Dictionary<Type, IEntityBuilderFactory> GateFactories { get; set; } = new() {
        [typeof(Constant)] = new InstantSpawnerFactory<ConstantSpawner>(),
        [typeof(DecisionDiagrams.Circuits.Input)] = new InstantSpawnerFactory<InputSpawner>(),
        [typeof(NotGate)] = new InstantSpawnerFactory<NotGateSpawner>(),
        [typeof(OrGate)] = new InstantSpawnerFactory<OrGateSpawner>(),
        [typeof(AndGate)] = new InstantSpawnerFactory<AndGateSpawner>(),
        [typeof(MuxGate)] = new InstantSpawnerFactory<MuxGateSpawner>()
    };

    public IEntityBuilderFactory ConnectionFactory { get; set; } = new CircuitConnectionFactory();
    
    public ICircuitElement Root { get; set; } = default!;
    public ILayout Layout { get; set; } = new EfficientSugiyamaLayout
    {
        Parameters =
        {
            Direction = LayoutDirection.LeftToRight,
            OptimizeWidth = true,
            LayerDistance = 1,
            VertexDistance = 2
        }
    };

    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var entities = new Dictionary<int, IEntity>();
        Spawn(Root, Layout.Arrange(Root), entities);
        
        return entities.Values;
    }

    private IEntity Spawn(ICircuitElement root, LayoutInfo layout, Dictionary<int, IEntity> cache)
    {
        if (cache.TryGetValue(root.Id, out var entity))
        { 
            return entity;
        }

        var position = -layout.Position(root.Id)!.Value;
        
        var spawnerEntity = Context.Instantiate(GateFactories[root.GetType()]
            .Create()
            .ConfigureComponent<Position>(x => x.Value = position + Position)
        );
        entity = spawnerEntity.GetRequiredComponent<Spawner>().Component!.Spawn().First();
        cache[root.Id] = entity;
        
        if (root is DecisionDiagrams.Circuits.Input rootInput)
        {
            var inputComponent = entity.GetRequiredComponent<Components.Circuits.Input>().Component!;
            inputComponent.VariableId = rootInput.InputId;
            inputComponent.Inverted = rootInput.Inverted;
            return entity;
        }

        if (root is Constant constantRoot)
        {
            entity.GetRequiredComponent<Components.Circuits.Constant>().Component!.Value = constantRoot.Value;
            return entity;
        }
        
        var gate = (ILogicGate)root;
        var gateComponent = entity.GetRequiredComponent<LogicGate>().Component!; 
        
        for (var i = 0; i < gate.Inputs.Length; i++)
        {
            var child = Spawn(gate.Inputs[i], layout, cache);
            var portPositionComponent = gateComponent.Ports[i].GetRequiredComponent<Position>().Component!;
            var childPositionComponent = child.GetRequiredComponent<Position>().Component!;
            
            if (MathF.Abs(childPositionComponent.Y - portPositionComponent.Y) < 2f)
            {
                childPositionComponent.Value = childPositionComponent.Value with
                {
                    Y = portPositionComponent.Value.Y
                };
            }
            
            var connectionEntity = Context.Instantiate(ConnectionFactory
                .Create()
                .ConfigureComponent<ChildOf>(x => x.Parent = gateComponent.Ports[i])
                .ConfigureComponent<Connection>(x => x.Target = child.GetRequiredComponent<LogicGate>().Component!.Ports[^1])
            );
            
            var connection = connectionEntity.GetRequiredComponent<Connection>().Component!;

            var jointPositionSet = false;
            
            foreach (var joint in layout.Joints(gate.Id, gate.Inputs[i].Id))
            {
                var actualJoint = -joint;

                if ((actualJoint + Position).X > portPositionComponent.Value.X)
                {
                    continue;
                }
                
                var jointComponent = connection.Split(actualJoint + Position);
                
                if (!jointPositionSet)
                {
                    var jointPositionComponent = jointComponent.Entity.GetRequiredComponent<Position>().Component!;
                    jointPositionComponent.Value = jointPositionComponent.Value with
                    {
                        Y = gateComponent.Ports[i].GetRequiredComponent<Position>().Component!.Value.Y
                    };
                    jointPositionSet = true;
                }
                
                connection = jointComponent.Connection2.GetRequiredComponent<Connection>()!;
            }
        }

        return entity;
    }
}