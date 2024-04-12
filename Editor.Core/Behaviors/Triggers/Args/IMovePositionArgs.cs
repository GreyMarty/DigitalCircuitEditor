using System.Numerics;

namespace Editor.Core.Behaviors.Triggers.Args;

public interface IMovePositionArgs : IPositionArgs
{
    public Vector2 OldPosition { get; }
}