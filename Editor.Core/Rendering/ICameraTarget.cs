using System.Numerics;

namespace Editor.Core.Rendering;

public interface ICameraTarget<T>
{
    public Vector2 Size { get; }
    public Vector2 DpiScale { get; }


    public void Update(T target, Vector2? dpiScale = null);
}