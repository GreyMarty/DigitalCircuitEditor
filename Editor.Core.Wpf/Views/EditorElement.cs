using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Editor.Core.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace Editor.Core.Wpf.Views;

public class EditorElement : UserControl
{
    protected EditorElement()
    {
        SetBindings();
        AddTriggers();

        RenderTransform = CreateTransformGroup();
    }

    private void SetBindings()
    {
        SetBinding(
            Canvas.LeftProperty,
            new Binding(nameof(EditorElementViewModel.PixelsX))
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
        
        SetBinding(
            Canvas.TopProperty,
            new Binding(nameof(EditorElementViewModel.PixelsY))
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
    }
    
    private void AddTriggers()
    {
        var mouseEnterEventTrigger = new EventTrigger("MouseEnter");
        
        var mouseEnterAction = new InvokeCommandAction()
        {
            CommandParameter = true
        };
        BindingOperations.SetBinding(
            mouseEnterAction,
            InvokeCommandAction.CommandProperty,
            new Binding(nameof(EditorElementViewModel.HoverCommand))
        );
        
        mouseEnterEventTrigger.Actions.Add(mouseEnterAction);
        Interaction.GetTriggers(this).Add(mouseEnterEventTrigger);

        var mouseLeaveEventTrigger = new EventTrigger("MouseLeave");
        
        var mouseLeaveAction = new InvokeCommandAction()
        {
            CommandParameter = false
        };
        BindingOperations.SetBinding(
            mouseLeaveAction,
            InvokeCommandAction.CommandProperty,
            new Binding(nameof(EditorElementViewModel.HoverCommand))
        );
        
        mouseLeaveEventTrigger.Actions.Add(mouseLeaveAction);
        Interaction.GetTriggers(this).Add(mouseLeaveEventTrigger);
    }
    
    private Transform CreateTransformGroup()
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(CreateScaleTransform(-2, -2));
        transformGroup.Children.Add(CreateTranslateTransform());
        transformGroup.Children.Add(CreateScaleTransform(-0.5, -0.5));
        return transformGroup;
    }

    private ScaleTransform CreateScaleTransform(double scaleX, double scaleY)
    {
        return new ScaleTransform(scaleX, scaleY);
    }

    private TranslateTransform CreateTranslateTransform()
    {
        var translateTransform = new TranslateTransform();
        
        BindingOperations.SetBinding(
            translateTransform,
            TranslateTransform.XProperty,
            new Binding("ActualWidth")
            {
                Source = this
            }
        );
        
        BindingOperations.SetBinding(
            translateTransform,
            TranslateTransform.YProperty,
            new Binding("ActualHeight")
            {
                Source = this
            }
        );
        
        return translateTransform;
    }
}