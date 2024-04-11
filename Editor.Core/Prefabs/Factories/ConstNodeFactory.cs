using System.Numerics;
using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories;

public class ConstNodeFactory<TConnectionType> : IEntityBuilderFactory 
    where TConnectionType : notnull 
{
    public virtual IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new RectangleShape
            {
                Width = 3,
                Height = 3
            })
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent<ConstNode<TConnectionType>>()
            .AddComponent<DragOnMouseMove>()
            .AddComponent<DestroyOnRequested>()
            .AddComponent(new ChangeFillOnHover
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent<NodeLabelToTextAdapter<TConnectionType>>()
            .AddComponent(new RequestPropertiesInspectorOnMouseButtonDown
            {
                Button = MouseButton.Right
            })
            .AddComponent<RequestRenderOnComponentChange>()
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
    }
}