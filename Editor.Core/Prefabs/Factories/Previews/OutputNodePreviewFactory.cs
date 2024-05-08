using System.Numerics;
using Editor.Component;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Previews;

public class OutputNodePreviewFactory : PreviewSpawnerFactoryBase
{
    public override IEntityBuilder Create()
    {
        var builder = base.Create()
            .AddComponent<BinaryDiagramOutputNodeSpawner>()
            .AddComponent(new LabeledRectangleRenderer()
            {
                Width = 3,
                Height = 3,
                Fill = new SKColor(255, 255, 255, 125),
                StrokeThickness = 0.2f,
                Stroke = new SKColor(0, 0, 0, 125),
                FontSize = 1.5f,
                Anchor = Vector2.One * 0.5f,
                Text = "F0"
            });

        return builder;
    }
}