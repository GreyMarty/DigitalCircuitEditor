using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories;

public class SelectionAreaFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<RectangleShape>()
            .AddComponent<SelectionArea>()
            .AddComponent<RectShapeToRendererAdapter>()
            .AddComponent(new RectangleRenderer
            {
                ZIndex = 100,
                Stroke = new SKColor(50, 125, 125, 50),
                StrokeThickness = 0.15f,
                Fill = new SKColor(100, 255, 255, 50)
            });

        builder
            .AddBehavior<DestroyBehavior, ITriggerArgs>(
                new MouseButtonUpTrigger
                {
                    Button = MouseButton.Left
                }
            )
            .AddBehavior<RequestRenderBehavior, ITriggerArgs>(
                new ComponentChangedTrigger<Position>(),
                new ComponentChangedTrigger<Renderer>()
            );

        return builder;
    }
}