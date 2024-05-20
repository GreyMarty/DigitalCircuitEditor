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

public class ConstNodeFactory : IEntityBuilderFactory
{
    private readonly bool _value = false;


    public ConstNodeFactory()
    {
    }

    public ConstNodeFactory(bool value)
    {
        _value = value;
    }
    
    
    public virtual IEntityBuilder Create()
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
            .AddComponent(new ConstNode
            {
                Value = _value
            })
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
                FontSize = 1.5f,
                Anchor = Vector2.One * 0.5f
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
            .AddBehavior<RequestPropertiesInspectorBehavior, ITriggerArgs>(
                new MouseButtonDownTrigger
                {
                    Button = MouseButton.Right,
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