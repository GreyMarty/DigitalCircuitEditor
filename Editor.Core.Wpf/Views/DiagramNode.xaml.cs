using Editor.Core.ViewModels;

namespace Editor.Core.Wpf.Views;

public partial class DiagramNode : EditorElement
{
    public DiagramNode()
    {
        DataContext = new DiagramNodeViewModel();
        InitializeComponent();
    }
}