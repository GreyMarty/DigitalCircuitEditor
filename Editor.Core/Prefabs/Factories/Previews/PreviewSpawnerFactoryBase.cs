using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Components;
using Editor.Core.Input;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Prefabs.Factories.Previews;

public abstract class PreviewSpawnerFactoryBase : IEntityBuilderFactory
{
    public virtual IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddBehavior<LockMouseBehavior, ITriggerArgs>(
                new InitTrigger()
            )
            .AddBehavior<UnlockMouseBehavior, ITriggerArgs>(
                new DestroyTrigger()
            )
            .AddBehavior<FollowMouseBehavior, IMovePositionArgs>(
                new MouseMoveTrigger()
            )
            .AddBehavior<SpawnBehavior, ITriggerArgs>(
                new MouseButtonDownTrigger
                {
                    Button = MouseButton.Left
                }
            )
            .AddBehavior<DestroyBehavior, ITriggerArgs>(
                new MouseButtonDownTrigger
                {
                    Button = MouseButton.Right
                }
            )
            .AddBehavior<RequestRenderBehavior, ITriggerArgs>(
                new ComponentChangedTrigger<Position>(),
                new ComponentChangedTrigger<Renderer>()
            );
    }
}