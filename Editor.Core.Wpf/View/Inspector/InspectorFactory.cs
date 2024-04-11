using System.Windows;
using Editor.Component;
using Editor.Core.Components.Diagrams;

namespace Editor.Core.Wpf.View.Inspector;

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
                case IConstNode:
                    return new ConstNodeInspector
                    {
                        DataContext = component
                    };
            }
        }

        return null;
    }
}