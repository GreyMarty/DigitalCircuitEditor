using System.Windows.Input;
using EditorMouseButton = Editor.Core.Input.MouseButton;

namespace Editor.Core.Wpf.Converters;

public static class MouseButtonConverter
{
    public static EditorMouseButton ToEditor(this MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => EditorMouseButton.Left,
            MouseButton.Middle => EditorMouseButton.Middle,
            MouseButton.Right => EditorMouseButton.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }
}