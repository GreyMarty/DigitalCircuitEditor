using System.Numerics;
using Editor.Component;

namespace Editor.Core.Behaviors.Triggers.Args;

public interface IPositionArgs : ITriggerArgs
{
    public Vector2 Position { get; }
}