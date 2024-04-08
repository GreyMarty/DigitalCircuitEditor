using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class DestroyOnMouseButtonDown : OnMouseButtonDownBehavior
{
    private Hoverable _hoverableComponent = default!;
    
    protected override void OnInit()
    {
        base.OnInit();

        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
    }

    protected override void OnMouseButtonDown(MouseButtonDown e)
    {
        if (!_hoverableComponent.Hovered)
        {
            return;
        }
        
        Context.Destroy(Entity);
    }
}