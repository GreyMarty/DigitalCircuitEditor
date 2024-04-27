using System.Numerics;

namespace Editor.Core.Layout;

public interface ILayoutNode
{
    public Vector2 Position { get; set; }
    public bool IsJoint { get; }
}

public class LayoutNode(bool isJoint = false) : ILayoutNode
{
    public Vector2 Position { get; set; }
    public bool IsJoint { get; } = isJoint;
}