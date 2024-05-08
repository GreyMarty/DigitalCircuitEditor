using System.Numerics;
using Editor.Component;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Previews;

public class BinaryDiagramNodePreviewFactory : PreviewSpawnerFactoryBase
{
    public override IEntityBuilder Create()
    {
        var builder = base.Create()
            .AddComponent<BinaryDiagramNodeSpawner>()
            .AddComponent(new LabeledCircleRenderer
            {
                Radius = 2,
                Fill = new SKColor(255, 255, 255, 125),
                StrokeThickness = 0.2f,
                Stroke = new SKColor(0, 0, 0, 125),
                FontSize = 1.5f,
                Anchor = Vector2.One * 0.5f,
                Text = "x0",
                ZIndex = 1000
            });

        return builder;
    }
}