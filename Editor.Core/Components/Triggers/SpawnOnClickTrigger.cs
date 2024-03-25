using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Triggers;

public class SpawnOnClickTrigger : OnClickTrigger
{
    private Spawner _spawnerComponent = default!;
    
    
    public override void Init(EditorWorld world, Entity entity)
    {
        _spawnerComponent = entity.GetRequiredComponent<Spawner>();
        
        base.Init(world, entity);
    }

    protected override void OnClick(MouseButtonDown e, bool hovered)
    {
        if (hovered && e.Button == MouseButton.Left)
        {
            _spawnerComponent.Spawn();
        }
    }
}