using System.Numerics;
using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public class NodeFactory : IEntityBuilderFactory
{
    public virtual IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new CircleShape
            {
                Radius = 2
            })
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent<DragOnMouseMove>()
            .AddComponent(new ChangeFillOnHover
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent<RequestRenderOnComponentChange>()
            .AddComponent(new LabeledCircleRenderer
            {
                Radius = 2,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
                FontSize = 2,
                Anchor = Vector2.One * 0.5f
            });
    }
}