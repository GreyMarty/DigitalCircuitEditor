using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Filters;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Circuits;

public abstract class LogicGateFactoryBase : IEntityBuilderFactory
{
    public virtual IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new RectangleShape()
            {
                Width = 3,
                Height = 3
            })
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent(new ChangeFillOnHover
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<ChangeStrokeOnSelect>();

        builder
            .AddBehavior<FollowMouseDeltaBehavior, IMovePositionArgs>(
                new MouseMoveTrigger
                {
                    Button = MouseButton.Left,
                    Filters = [ new SelectedFilter(), new MouseUnlockedFilter() ],
                    FilterMode = TriggerFilterMode.All
                }
            )
            .AddBehavior<RequestRenderBehavior, ITriggerArgs>(
                new ComponentChangedTrigger<Position>(),
                new ComponentChangedTrigger<Renderer>()
            );

        return builder;
    }
}