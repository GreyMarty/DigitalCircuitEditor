using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Editor.ViewModel;

namespace Editor.View.Wpf.Controls;

public partial class ToolBar : UserControl
{
    public ToolBar()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }


    public ToolBarViewModel ViewModel
    {
        get => (ToolBarViewModel)DataContext;
        set => DataContext = value;
    }
    
    
    private void ItemsControl_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (ItemsControl.ContainerFromElement((ItemsControl)sender, e.OriginalSource as DependencyObject) is not FrameworkElement item)
        {
            return;
        }

        ViewModel.SelectItem((ToolBarItemViewModel)item.DataContext);
    }
}