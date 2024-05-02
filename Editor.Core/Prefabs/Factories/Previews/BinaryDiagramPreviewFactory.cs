using System.Numerics;
using Editor.Component;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Previews;

public class BinaryDiagramPreviewFactory : PreviewSpawnerFactoryBase
{
    public override IEntityBuilder Create()
    {
        var builder = base.Create()
            .AddComponent<BinaryDiagramSpawner>()
            .AddComponent(new LabeledCircleRenderer
            {
                Radius = 2,
                Fill = new SKColor(255, 255, 255, 125),
                StrokeThickness = 0.2f,
                Stroke = new SKColor(0, 0, 0, 125),
                FontSize = 2,
                Anchor = Vector2.One * 0.5f,
                Text = "Y",
                ZIndex = 1000
            });

        return builder;
    }
}