using Editor.Core.ViewModels;

namespace Editor.Core.Wpf.Views;

public partial class GhostDiagramNode : EditorElement
{
    public GhostDiagramNode()
    {
        DataContext = new DiagramNodeViewModel();
        InitializeComponent();
    }
}