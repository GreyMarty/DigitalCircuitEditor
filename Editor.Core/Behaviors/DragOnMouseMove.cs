﻿using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors;

public class DragOnMouseMove : OnMouseMoveBehavior
{
    private Position _positionComponent = default!;
    private Selectable? _selectableComponent;

    
    protected override void OnInit()
    {
        base.OnInit();
        
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        _selectableComponent = Entity.GetComponent<Selectable>()?.Component;
    }

    protected override void OnMouseMove(MouseMove e)
    {
        if (Context.MouseLocked)
        {
            return;
        }
        
        if (_selectableComponent?.Selected != true || e.Button != MouseButton.Left)
        {
            return;
        }
        
        var delta = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels) - e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels);
        _positionComponent.Value += delta;
    }
}