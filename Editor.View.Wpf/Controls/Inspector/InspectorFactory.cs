using System.Windows;
using Editor.Component;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;

namespace Editor.View.Wpf.Controls.Inspector;

public interface IInspectorFactory
{
    public UIElement? Create(IEntity entity);
}

public class InspectorFactory : IInspectorFactory
{
    public UIElement? Create(IEntity entity)
    {
        foreach (var component in entity.Components)
        {
            switch (component)
            {
                case ConstNode:
                    return new ConstNodeInspector
                    {
                        DataContext = component
                    };
                
                case BinaryDiagramNode:
                    return new BinaryDiagramNodeInspector
                    {
                        DataContext = component
                    };
            }
        }

        return null;
    }
}