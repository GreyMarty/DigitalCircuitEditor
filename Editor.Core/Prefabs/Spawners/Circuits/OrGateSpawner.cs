using System.Numerics;
using Editor.Core.Prefabs.Factories.Circuits;

namespace Editor.Core.Prefabs.Spawners.Circuits;

public class OrGateSpawner : LogicGateSpawnerBase<OrGateFactory>
{
    protected override IEnumerable<Vector2> GetPortPositions(float width, float height)
    {
        return
        [
            new Vector2(-width / 2, -height / 4),
            new Vector2(-width / 2, height / 4),
            new Vector2(width / 2, 0)
        ];
    }
}