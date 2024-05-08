using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Circuits;
using Editor.Core.Prefabs.Factories.Circuits;
using Editor.Core.Shapes;

namespace Editor.Core.Prefabs.Spawners.Circuits;

public abstract class LogicGateSpawnerBase<TFactory> : Spawner
    where TFactory : IEntityBuilderFactory, new()
{
    public IEntityBuilderFactory GateFactory { get; set; } = new TFactory();
    public IEntityBuilderFactory PortFactory { get; set; } = new CircuitPortFactory();
    

    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var result = new List<IEntity>();
        
        var circuitEntity = context.Instantiate(GateFactory
            .Create()
            .ConfigureComponent<Position>(x => x.Value = Position)
        );
        result.Add(circuitEntity);

        var gate = circuitEntity.Components.OfType<LogicGate>().Single();
        var shape = circuitEntity.GetRequiredComponent<RectangleShape>().Component!;

        var ports = SpawnPorts(circuitEntity, GetPortPositions(shape.Width, shape.Height));
        result.AddRange(ports);
        
        ports.CopyTo(gate.Ports, 0);
        
        return result;
    }

    protected abstract IEnumerable<Vector2> GetPortPositions(float width, float height);
    
    private IEntity[] SpawnPorts(IEntity parent, IEnumerable<Vector2> positions)
    {
        return positions
            .Select(position => Context.Instantiate(PortFactory.Create()
                .ConfigureComponent<Position>(x => x.Value = position)
                .ConfigureComponent<ChildOf>(x => x.Parent = parent)
            ))
            .ToArray();
    }
}