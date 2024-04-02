using System.Numerics;
using SkiaSharp;

namespace Editor.Core.Rendering;

public class SKCameraTarget : ICameraTarget<SKImageInfo>
{
    public Vector2 Size { get; private set; }
    
    
    public void Update(SKImageInfo target)
    {
        Size = new Vector2(target.Width, target.Height);
    }
}