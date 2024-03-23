using System.Numerics;
using Editor.Core.Input;
using TinyMessenger;

namespace Editor.Core.Events;

public class MouseButtonUp(object sender, MouseButton button, Vector2 position, ModKeys modKeys = 0) : TinyMessageBase(sender)
{
    public MouseButton Button { get; } = button;
    public Vector2 Position { get; } = position;
    public ModKeys ModKeys { get; } = modKeys;
}