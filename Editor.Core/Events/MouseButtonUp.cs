using System.Numerics;
using Editor.Core.Input;
using Editor.Core.Rendering;
using TinyMessenger;

namespace Editor.Core.Events;

public record MouseButtonUp(
    object Sender,
    MouseButton Button,
    Vector2 PositionPixels,
    IPositionConverter PositionConverter,
    ModKeys ModKeys = 0
) : ITinyMessage;