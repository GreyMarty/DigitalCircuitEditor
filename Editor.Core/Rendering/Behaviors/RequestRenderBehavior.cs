using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Rendering.Renderers;
using TinyMessenger;

namespace Editor.Core.Rendering.Behaviors;

public class RequestRenderBehavior : EditorComponentBase
{
    private List<EditorComponentBase?> _components = [];
    private ITinyMessengerHub _eventBus = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _eventBus = context.EventBus;
        
        _components.Add(entity.GetComponent<Position>()?.Component);
        _components.Add(entity.GetComponent<Hoverable>()?.Component);
        _components.Add(entity.GetComponent<Selectable>()?.Component);
        _components.Add(entity.GetComponent<Renderer>()?.Component);
        
        _components = _components
            .Where(x => x is not null)
            .ToList();
        
        foreach (var component in _components)
        {
            component!.PropertyChanged += Component_OnPropertyChanged;
        }
    }

    protected override void OnDestroy()
    {
        foreach (var component in _components)
        {
            component!.PropertyChanged -= Component_OnPropertyChanged;
        }
    }

    private void Component_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _eventBus.Publish(new RenderRequested(this));
    }
}