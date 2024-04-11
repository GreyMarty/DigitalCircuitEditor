using System.ComponentModel;
using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Rendering.Renderers;
using TinyMessenger;

namespace Editor.Core.Behaviors;

public class RequestRenderOnComponentChange : EditorComponentBase
{
    private EditorComponentBase[] _components = [];
    
    
    protected override void OnInit()
    {
        var components = new List<EditorComponentBase?>
        {
            Entity.GetComponent<Position>()?.Component,
            Entity.GetComponent<Hoverable>()?.Component,
            Entity.GetComponent<Selectable>()?.Component,
            Entity.GetComponent<Renderer>()?.Component
        };

        _components = components
            .Where(x => x is not null)
            .ToArray()!;
        
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
        Context.EventBus.Publish(new RenderRequested(this));
    }
}