using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Components;

namespace Editor.Core.Behaviors;

public class FollowMouseDeltaBehavior : BehaviorBase<EditorContext, IMovePositionArgs>
{
    private Position _positionComponent = default!;

    
    protected override void OnInit()
    {
        base.OnInit();
        
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
    }

    protected override void Perform(IMovePositionArgs e)
    {
        var delta = e.Position - e.OldPosition;
        _positionComponent.Value += delta;
    }
}

public class FollowMouseBehavior : BehaviorBase<EditorContext, IMovePositionArgs>
{
    private Position _positionComponent = default!;

    
    protected override void OnInit()
    {
        base.OnInit();
        
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
    }

    protected override void Perform(IMovePositionArgs e)
    {
        _positionComponent.Value = e.Position;
    }
}