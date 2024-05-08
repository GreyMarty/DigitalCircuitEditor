using System.Numerics;
using Editor.Core.Prefabs.Factories.Circuits;

namespace Editor.Core.Prefabs.Spawners.Circuits;

public class InputSpawner : LogicGateSpawnerBase<InputFactory>
{
    protected override IEnumerable<Vector2> GetPortPositions(float width, float height)
    {
        return
        [
            new Vector2(width / 2, 0)
        ];
    }
}