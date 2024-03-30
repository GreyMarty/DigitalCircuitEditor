using System.Windows.Media;
using Editor.Core.ViewModels;

namespace Editor.Core.Wpf.Views;

public partial class ConnectionLine : EditorElement
{
    public ConnectionLine()
    {
        DataContext = new ConnectionViewModel();
        RenderTransform = Transform.Identity;
        InitializeComponent();
        
    }
}