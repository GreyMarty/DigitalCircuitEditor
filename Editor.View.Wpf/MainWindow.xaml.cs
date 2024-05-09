using Editor.Core.Prefabs.Factories.Previews;
using Editor.View.Wpf.Controls.Toolbar.Previews;
using Editor.View.Wpf.Services;
using Editor.ViewModel;
using MahApps.Metro.Controls;

namespace Editor.View.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    private readonly MainViewModel _viewModel = new();
    
    
    public MainWindow()
    {
        DataContext = _viewModel;
        _viewModel.Editor.FilePrompt = new FilePathPrompt();
        _viewModel.Editor.PreviewService = new CircuitPreviewService(this);
        _viewModel.ToolBar.Items = [
            new ToolBarItemViewModel
            {
                Hotkey = '1',
                Preview = new DiagramNodePreview(),
                Factory = new BinaryDiagramNodePreviewFactory()
            },
            new ToolBarItemViewModel
            {
                Hotkey = '2',
                Preview = new ConstNodePreview(),
                Factory = new ConstNodePreviewFactory()
            },
            new ToolBarItemViewModel
            {
                Hotkey = '3',
                Preview = new LabelPreview()
            }
        ];
        
        InitializeComponent();
    }
}