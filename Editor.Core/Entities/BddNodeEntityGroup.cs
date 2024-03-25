using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Triggers;
using Editor.Core.Presentation;
using Editor.Core.ViewModels;

namespace Editor.Core.Entities;

public class BddNodeEntityGroup<TColor> : IEntityGroup
{
    private readonly IPalette<TColor> _palette;
    
    
    public BddNodeEntityGroup(IPalette<TColor> palette)
    {
        _palette = palette;
    }

    
    public IEnumerable<Entity> GetEntities()
    {
        var entities = new List<Entity>();
        
        var node = new Entity();
        node.AddComponent<Position>();
        node.AddComponent<Hoverable>();
        node.AddComponent<Selectable>();
        node.AddComponent<DragOnMouseMoveTrigger>();
        node.AddComponent<BddNodeViewModel<TColor>>(x => x.Palette = _palette);
        entities.Add(node);
        
        return entities;
    }
}