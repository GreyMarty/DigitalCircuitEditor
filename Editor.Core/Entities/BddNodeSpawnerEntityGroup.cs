using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Triggers;
using Editor.Core.Presentation;
using Editor.Core.ViewModels;

namespace Editor.Core.Entities;

public class BddNodeSpawnerEntityGroup<TColor> : IEntityGroup
{
    private readonly IPalette<TColor> _palette;
    
    
    public BddNodeSpawnerEntityGroup(IPalette<TColor> palette)
    {
        _palette = palette;
    }


    public IEnumerable<Entity> GetEntities()
    {
        var entity = new Entity();
        entity.AddComponent<Position>();
        entity.AddComponent<Hoverable>();
        entity.AddComponent<Spawner>(x =>
        {
            x.Spawnables = new BddNodeEntityGroup<TColor>(_palette);
            x.DestroyOnSpawn = true;
        });
        entity.AddComponent<SpawnOnClickTrigger>();
        entity.AddComponent<ArrowSpawnerViewModel<TColor>>(x => x.Palette = _palette);
        
        return new[] { entity };
    }
}