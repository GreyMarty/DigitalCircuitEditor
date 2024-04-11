using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors;

public abstract class OnMouseButtonDownBehavior : EditorComponentBase
{
    public MouseButton Button { get; set; } = MouseButton.Left;
    
    
    protected override void OnInit()
    {
        Events.Subscribe<MouseButtonDown>(e =>
        {
            if (e.Button == Button)
            {
                OnMouseButtonDown(e);
            }
        });
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseButtonDown>();
    }
    
    protected abstract void OnMouseButtonDown(MouseButtonDown e);
}