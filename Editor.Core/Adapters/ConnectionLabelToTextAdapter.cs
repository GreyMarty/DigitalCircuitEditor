using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Adapters;

public class ConnectionLabelToTextAdapter : EditorComponentBase
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
        _rendererComponent.Text = connectionComponent.Label;
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Connection component)
        {
            OnConnectionComponentChanged(component);
        }
    }
}