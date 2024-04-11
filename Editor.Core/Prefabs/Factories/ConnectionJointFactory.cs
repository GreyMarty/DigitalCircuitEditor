using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
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
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new CircleShape
            {
                Radius = 0.5f
            })
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent<ConnectionJoint>()
            .AddComponent<DragOnMouseMove>()
            .AddComponent<DestroyOnRequested>()
            .AddComponent<DestroyWith>()
            .AddComponent<ChangeStrokeOnSelect>()
            .AddComponent<RequestRenderOnComponentChange>()
            .AddComponent(new CircleRenderer
            {
                Radius = 0.5f,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
            });
    }
}