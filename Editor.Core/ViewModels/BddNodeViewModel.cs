using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class BddNodeViewModel : ViewModelComponentBase
{
    public Position Position { get; private set; } = default!;
    public Hoverable Hoverable { get; private set; } = default!;
    
    public string? Label { get; set; }


    public override void Init(EditorWorld world, Entity entity)
    {
        Position = entity.GetRequiredComponent<Position>();
        Hoverable = entity.GetRequiredComponent<Hoverable>();
    }
}
