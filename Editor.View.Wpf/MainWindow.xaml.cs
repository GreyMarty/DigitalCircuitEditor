using Editor.Core.Prefabs.Factories.Previews;
using Editor.View.Wpf.Controls.Toolbar.Previews;
using Editor.ViewModel;
using MahApps.Metro.Controls;

namespace Editor.View.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    private readonly ToolBarViewModel _toolBarViewModel;
    
    
    public MainWindow()
    {
        _toolBarViewModel = new ToolBarViewModel()
        {
            Items = [
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
            ]
        };
        
        _toolBarViewModel.ItemSelected += ToolBar_OnItemSelected;
        
        InitializeComponent();
    }

    ~MainWindow()
    {
        _toolBarViewModel.ItemSelected -= ToolBar_OnItemSelected;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        ToolBar.DataContext = _toolBarViewModel;
    }

    private void ToolBar_OnItemSelected(ToolBarViewModel sender, ToolBarItemViewModel? item)
    {
        var editorViewModel = (EditorViewModel)Editor.DataContext;
        editorViewModel.OnToolBarItemSelected(sender, item);
    }
}