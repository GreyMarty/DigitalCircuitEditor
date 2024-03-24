using System.ComponentModel;
using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components;

public class Selectable : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    private Hoverable _hoverableComponent = default!;
    
    
    public bool Selected { get; set; }
        
        
    public event PropertyChangedEventHandler? PropertyChanged;


    public override void Init(EditorWorld world, Entity entity)
    {
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>();

        world.EventBus.Subscribe<MouseButtonDown>(World_MouseButtonDown);
    }
    
    private void World_MouseButtonDown(MouseButtonDown e)
    {
        if (e.Button != MouseButton.Left)
        {
            return;
        }
        
        Selected = _hoverableComponent.Hovered;
    }
}
