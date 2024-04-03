using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor.Core.Wpf.View;

public partial class EditorEntitiesListItem : UserControl
{
    public EditorEntitiesListItem()
    {
        InitializeComponent();
    }
    
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var data = new DataObject();
            data.SetData("Object", DataContext);

            DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
        }
    }
}