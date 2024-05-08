using System.Numerics;
using Editor.Core.Prefabs.Factories.Circuits;

namespace Editor.Core.Prefabs.Spawners.Circuits;

public class NotGateSpawner : LogicGateSpawnerBase<NotGateFactory>
{
    protected override IEnumerable<Vector2> GetPortPositions(float width, float height)
    {
        return
        [
            new Vector2(-width / 2, 0),
            new Vector2(width / 2 + width * 0.15f, 0)
        ];
    }
}