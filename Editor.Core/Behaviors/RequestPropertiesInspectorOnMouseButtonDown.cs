using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class RequestPropertiesInspectorOnMouseButtonDown : OnMouseButtonDownBehavior
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
        
        Context.EventBus.Publish(new RequestPropertiesInspector(Entity));
    }
}