using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Adapters;

public class ConnectionTypeToStrokeStyleAdapter : EditorComponentBase
{
    private LabeledShapeRenderer _rendererComponent = default!;


    protected override void OnInit()
    {
        _rendererComponent = Entity.GetRequiredComponent<LabeledShapeRenderer>()!;
        
        Entity.ComponentChanged += Entity_OnComponentChanged;
        OnConnectionComponentChanged(Entity.GetRequiredComponent<Connection>()!);
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void OnConnectionComponentChanged(Connection connectionComponent)
    {
        if (connectionComponent.Type == ConnectionType.False)
        {
            _rendererComponent.StrokePathEffect = SKPathEffect.CreateDash([0.5f, 0.5f], 0);
        }
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Connection component && e.PropertyName == nameof(component.Type))
        {
            OnConnectionComponentChanged(component);
        }
    }
}