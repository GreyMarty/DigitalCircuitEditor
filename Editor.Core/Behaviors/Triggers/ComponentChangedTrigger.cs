using System.ComponentModel;
using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;

namespace Editor.Core.Behaviors.Triggers;

public class ComponentChangedTrigger<TComponent> : TriggerBase<EditorContext, ComponentChangedTriggerArgs> where TComponent : ComponentBase
{
    protected override void OnInit()
    {
        Entity.ComponentChanged += Entity_OnComponentChanged;
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is TComponent component)
        {
            OnFired(new ComponentChangedTriggerArgs(component, e.PropertyName));
        }
    }
}