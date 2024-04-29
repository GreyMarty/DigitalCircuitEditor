using System.Numerics;
using Editor.Component;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Previews;

public class ConstNodePreviewFactory : PreviewSpawnerFactoryBase
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<SimpleSpawner<ConstNodeFactory>>()
            .AddComponent(new LabeledRectangleRenderer()
            {
                Width = 3,
                Height = 3,
                Fill = new SKColor(255, 255, 255, 125),
                StrokeThickness = 0.2f,
                Stroke = new SKColor(0, 0, 0, 125),
                FontSize = 2,
                Anchor = Vector2.One * 0.5f,
                Text = "0"
            });
    }
}