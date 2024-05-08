using System.Numerics;
using Editor.Core.Prefabs.Factories.Circuits;

namespace Editor.Core.Prefabs.Spawners.Circuits;

public class MuxGateSpawner : LogicGateSpawnerBase<MuxGateFactory>
{
    protected override IEnumerable<Vector2> GetPortPositions(float width, float height)
    {
        return 
        [
            new Vector2(-width / 2, -height / 4),
            new Vector2(-width / 2, height / 4),
            new Vector2(0, height / 2),
            new Vector2(width / 2, 0),
        ];
    }
}