using System.Windows.Media;
using Editor.Core.ViewModels;

namespace Editor.Core.Wpf.Views;

public partial class GhostConnectionLine : EditorElement
{
    public GhostConnectionLine()
    {
        DataContext = new ConnectionViewModel();
        RenderTransform = Transform.Identity;
        InitializeComponent();
        
    }
}