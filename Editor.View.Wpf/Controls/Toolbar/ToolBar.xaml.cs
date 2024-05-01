using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Editor.ViewModel;

namespace Editor.View.Wpf.Controls;

public partial class ToolBar : UserControl
{
    public ToolBar()
    {
        InitializeComponent();
    }
    
    private void ItemsControl_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is not ToolBarViewModel viewModel)
        {
            return;
        }
        
        if (ItemsControl.ContainerFromElement((ItemsControl)sender, e.OriginalSource as DependencyObject) is not FrameworkElement item)
        {
            return;
        }

        viewModel.SelectItem((ToolBarItemViewModel)item.DataContext);
    }
}