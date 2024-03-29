using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class BddNodeViewModel : VisualElementViewModel
{
    public ComponentRef<Position> Position { get; private set; } = default!;
    public ComponentRef<Hoverable> Hoverable { get; private set; } = default!;
    
    public string? Label { get; set; }


    public override void Init(EditorWorld world, IEntity entity)
    {
        Position = entity.GetRequiredComponent<Position>();
        Hoverable = entity.GetRequiredComponent<Hoverable>();
        
        base.Init(world, entity);
    }
}
