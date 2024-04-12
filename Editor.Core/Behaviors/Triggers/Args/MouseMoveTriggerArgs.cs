using System.Numerics;
using Editor.Core.Input;

namespace Editor.Core.Behaviors.Triggers.Args;

public record MouseMoveTriggerArgs(MouseButton Button, Vector2 OldPosition, Vector2 Position) : IMouseButtonArgs, IMovePositionArgs;