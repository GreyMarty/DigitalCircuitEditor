using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor.Core.Wpf.View;

public partial class EditorEntitiesList : UserControl
{
    public event SelectionChangedEventHandler? SelectionChanged;
    
    
    public EditorEntitiesList()
    {
        InitializeComponent();
    }

    private void List_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (ItemsControl.ContainerFromElement((ListBox)sender, e.OriginalSource as DependencyObject) is not ListBoxItem item)
        {
            return;
        }
        
        SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(e.RoutedEvent, Array.Empty<object>(), new[] { item.DataContext }));
    }
}