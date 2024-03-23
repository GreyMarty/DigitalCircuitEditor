using System.Numerics;
using TinyMessenger;

namespace Editor.Core.Events;

public class MouseWheel(object sender, float delta, Vector2 position) : TinyMessageBase(sender)
{
    public float Delta { get; } = delta;
    public Vector2 Position { get; } = position;
}