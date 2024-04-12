using System.Numerics;
using Editor.Component;
using Editor.Core.Input;

namespace Editor.Core.Behaviors.Triggers.Args;

public record MouseButtonTriggerArgs(MouseButton Button, Vector2 Position) : IMouseButtonArgs, IPositionArgs;