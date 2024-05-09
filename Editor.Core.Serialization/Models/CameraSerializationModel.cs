using System.Numerics;
using Editor.Core.Rendering;

namespace Editor.Core.Serialization.Models;

internal class CameraSerializationModel
{
    public Vector2 Position { get; set; }
    public float Scale { get; set; }


    public static CameraSerializationModel From(Camera camera) => new()
    {
        Position = camera.Position,
        Scale = camera.Scale
    };
}