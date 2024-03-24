using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class BddNodeViewModel<TColor> : VisualElementViewModel<TColor>
{
    public Position Position { get; private set; } = default!;
    public new Hoverable Hoverable { get; private set; } = default!;
    
    public string? Label { get; set; }


    public override void Init(EditorWorld world, Entity entity)
    {
        Position = entity.GetRequiredComponent<Position>();
        Hoverable = entity.GetRequiredComponent<Hoverable>();
        
        base.Init(world, entity);
    }
}
