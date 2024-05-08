using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Components.Circuits;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Adapters;

public class ConstantToTextAdapter : EditorComponentBase
{
    private LabeledShapeRenderer _rendererComponent = default!;
    
    protected override void OnInit()
    {
        _rendererComponent = Entity.GetRequiredComponent<LabeledShapeRenderer>()!;
     
        Entity.ComponentChanged += Entity_OnComponentChanged;
        OnConstantComponentChanged(Entity.GetRequiredComponent<Constant>()!);
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void OnConstantComponentChanged(Constant constantComponent)
    {
        _rendererComponent.Text = $"{(constantComponent.Value ? 1 : 0)}";
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Constant component)
        {
            OnConstantComponentChanged(component);
        }
    }
}