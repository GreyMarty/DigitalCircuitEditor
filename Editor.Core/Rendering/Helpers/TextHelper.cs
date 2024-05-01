using System.Numerics;
using SkiaSharp;

namespace Editor.Core.Rendering.Helpers;

public static class TextHelper
{
    public static void DrawText(Camera camera, SKCanvas canvas, string text, SKFont font, SKPaint paint, Vector2 anchor)
    {
        const float scaleFactor = 10;

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        font.Size *= scaleFactor;
        font.MeasureText(text.Select(c => (ushort)c).ToArray(), out var rect);

        var origin = new SKPoint(
            -rect.Left - rect.Width * anchor.X,
            -rect.Top - rect.Height * anchor.Y
        );

        using var textBlob = SKTextBlob.Create(text, font, origin);

        canvas.Save();
        canvas.Scale(1 / scaleFactor);
        
        canvas.DrawText(textBlob, 0, 0, paint);
        
        canvas.Restore();

        font.Size /= scaleFactor;
    }
}