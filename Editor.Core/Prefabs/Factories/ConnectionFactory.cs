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

public class ConnectionFactory : IEntityBuilderFactory
{
    public virtual IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new LineShape
            {
                Thickness = 0.4f
            })
            .AddComponent<Selectable>()
            .AddComponent<Hoverable>()
            .AddComponent(new ChildOf
            {
                DestroyWithParent = true
            })
            .AddComponent<Connection>()
            .AddComponent<ConnectionToLineShapeAdapter>()
            .AddComponent<ConnectionTypeToStrokeStyleAdapter>()
            .AddComponent<LineShapeToRendererAdapter>()
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent(new LabeledLineRenderer
            {
                Stroke = SKColors.Black,
                StrokeThickness = 0.2f,
                Anchor = new Vector2(0.5f, 0.5f)
            });

        builder
            .AddBehavior<DestroyBehavior, ITriggerArgs>(
                new EventTrigger<DestroyRequested>
                {
                    Filters = [ new SelectedFilter() ]
                }
            )
            .AddBehavior<CreateJointBehavior, IPositionArgs>(
                new MouseDoubleClickTrigger
                {
                    Button = MouseButton.Left,
                    Filters = [ new HoveredFilter() ]
                }
            )
            .AddBehavior<RequestRenderBehavior, ITriggerArgs>(
                new ComponentChangedTrigger<Position>(),
                new ComponentChangedTrigger<Renderer>()
            );

        return builder;
    }
}