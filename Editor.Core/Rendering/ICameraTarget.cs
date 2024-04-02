using System.Numerics;

namespace Editor.Core.Rendering;

public interface ICameraTarget<T>
{
    public Vector2 Size { get; }


    public void Update(T target);
}