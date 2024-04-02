using System.Numerics;
using Editor.Core.Input;
using Editor.Core.Rendering;
using TinyMessenger;

namespace Editor.Core.Events;

public record MouseMove(
    object Sender,
    Vector2 OldPositionPixels,
    Vector2 NewPositionPixels,
    IPositionConverter PositionConverter,
    MouseButton Button = 0,
    ModKeys ModKeys = 0
) : ITinyMessage;