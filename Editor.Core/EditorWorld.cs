using System.Numerics;
using Editor.Component;
using Editor.Core.Converters;

namespace Editor.Core;

public class EditorWorld : World
{
    public EditorWorld(IUnitsToPixelsConverter? positionConverter = null)
    {
        PositionConverter = positionConverter;
    }

    public IUnitsToPixelsConverter? PositionConverter { get; }
}