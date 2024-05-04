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

namespace Editor.Core.Prefabs.Factories;

public class ConnectionJointFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new CircleShape
            {
                Radius = 1f
            })
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent<ConnectionJoint>()
            .AddComponent<DestroyWith>()
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent(new CircleRenderer
            {
                Radius = 0.2f,
                Fill = SKColors.Black,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
                ZIndex = 10
            });

        builder
            .AddBehavior<FollowMouseDeltaBehavior, IMovePositionArgs>(
                new MouseMoveTrigger
                {
                    Button = MouseButton.Left,
                    Filters = [ new SelectedFilter(), new MouseUnlockedFilter() ],
                    FilterMode = TriggerFilterMode.All
                }
            )
            .AddBehavior<DestroyBehavior, ITriggerArgs>(
                new EventTrigger<DestroyRequested>
                {
                    Filters = [ new SelectedFilter() ]
                }
            )
            .AddBehavior<RequestRenderBehavior, ITriggerArgs>(
                new ComponentChangedTrigger<Position>(),
                new ComponentChangedTrigger<Renderer>()
            );

        return builder;
    }
}