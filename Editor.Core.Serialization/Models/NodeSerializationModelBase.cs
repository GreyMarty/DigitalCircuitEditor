using System.Numerics;

namespace Editor.Core.Serialization.Models;

internal abstract class NodeSerializationModelBase
{
    public Guid Id { get; set; }
    public Vector2 Position { get; set; }
}