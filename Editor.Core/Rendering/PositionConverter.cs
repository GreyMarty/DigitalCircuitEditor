using System.Numerics;

namespace Editor.Core.Rendering;

public interface IPositionConverter
{
    public Vector2 ScreenToCameraSpace(Vector2 point);
    public Vector2 ScreenToWorldSpace(Vector2 point);
}

public class PositionConverter(Camera camera) : IPositionConverter
{
    private readonly Camera _camera = camera;


    public Vector2 ScreenToCameraSpace(Vector2 point) => (point - _camera.SizePixels / 2) / _camera.PixelsPerUnit;
    public Vector2 ScreenToWorldSpace(Vector2 point) => (ScreenToCameraSpace(point) - _camera.Position) / _camera.Scale;
}