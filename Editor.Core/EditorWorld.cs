using System.Numerics;
using Editor.Component;

namespace Editor.Core;

public class EditorWorld : World
{
    public EditorWorld(IUnitsToPixelsConverter? positionConverter = null)
    {
        PositionConverter = positionConverter;
    }

    public IUnitsToPixelsConverter? PositionConverter { get; }
}