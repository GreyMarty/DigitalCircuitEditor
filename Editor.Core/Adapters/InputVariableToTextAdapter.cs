using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Adapters;

public class InputVariableToTextAdapter : EditorComponentBase
{
    private LabeledShapeRenderer _rendererComponent = default!;
    
    protected override void OnInit()
    {
        _rendererComponent = Entity.GetRequiredComponent<LabeledShapeRenderer>()!;
     
        Entity.ComponentChanged += Entity_OnComponentChanged;
        OnInputComponentChanged(Entity.GetRequiredComponent<Components.Circuits.Input>()!);
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void OnInputComponentChanged(Components.Circuits.Input inputComponent)
    {
        _rendererComponent.Text = $"{(inputComponent.Inverted ? "~" : "")}x{inputComponent.VariableId}";
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Components.Circuits.Input component)
        {
            OnInputComponentChanged(component);
        }
    }
}