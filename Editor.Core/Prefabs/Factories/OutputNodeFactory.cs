using System.Numerics;
using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Filters;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories;

public class OutputNodeFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new RectangleShape
            {
                Width = 3,
                Height = 3
            })
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent<OutputNode>()
            .AddComponent(new ChangeFillOnHover
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent<NodeLabelToTextAdapter>()
            .AddComponent(new LabeledRectangleRenderer()
            {
                Width = 3,
                Height = 3,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
                FontSize = 2,
                Anchor = Vector2.One * 0.5f
            });

        builder
            .AddBehavior<FollowMouseBehavior, IMovePositionArgs>(
                new MouseMoveTrigger()
                {
                    Button = MouseButton.Left,
                    Filters = [ new SelectedFilter() ]
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