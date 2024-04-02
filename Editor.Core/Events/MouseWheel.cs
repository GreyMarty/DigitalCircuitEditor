using System.Numerics;
using Editor.Core.Rendering;
using TinyMessenger;

namespace Editor.Core.Events;

public record MouseWheel(
    object Sender,
    float Delta,
    Vector2 PositionPixels,
    IPositionConverter PositionConverter
) : ITinyMessage;