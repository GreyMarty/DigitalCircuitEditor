using System.Numerics;
using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Input;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories;

public class ConnectionFactory<TConnection> : IEntityBuilderFactory
    where TConnection : Connection, new()
{
    public virtual IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
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
            .AddComponent<TConnection>()
            .AddComponent<DestroyOnRequested>()
            .AddComponent<CreateJointOnMouseDoubleClick>()
            .AddComponent<ConnectionLabelToTextAdapter>()
            .AddComponent<ConnectionToLineShapeAdapter>()
            .AddComponent<LineShapeToRendererAdapter>()
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent<RequestRenderOnComponentChange>()
            .AddComponent(new LabeledLineRenderer
            {
                Stroke = SKColors.Black,
                StrokeThickness = 0.2f,
                Anchor = new Vector2(0.5f, 0.5f)
            });
    }
}