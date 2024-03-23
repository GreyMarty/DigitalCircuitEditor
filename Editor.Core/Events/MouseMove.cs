using System.Numerics;
using Editor.Core.Input;
using TinyMessenger;

namespace Editor.Core.Events;

public class MouseMove(object sender, Vector2 oldPosition, Vector2 newPosition, MouseButton button = 0, ModKeys modKeys = 0) : TinyMessageBase(sender)
{
    public Vector2 OldPosition { get; } = oldPosition;
    public Vector2 NewPosition { get; } = newPosition;
    public MouseButton Button { get; } = button;
    public ModKeys ModKeys { get; } = modKeys;
}