using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Adapters;

public class NodeLabelToTextAdapter : EditorComponentBase
{
    private LabeledShapeRenderer _rendererComponent = default!;
    
    protected override void OnInit()
    {
        _rendererComponent = Entity.GetRequiredComponent<LabeledShapeRenderer>()!;
     
        Entity.ComponentChanged += Entity_OnComponentChanged;
        OnNodeComponentChanged(Entity.GetRequiredComponent<Node>()!);
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void OnNodeComponentChanged(Node nodeComponent)
    {
        _rendererComponent.Text = nodeComponent.Label;
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Node component)
        {
            OnNodeComponentChanged(component);
        }
    }
}