using Editor.DecisionDiagrams.Circuits;
using Editor.ViewModel;
using MahApps.Metro.Controls;

namespace Editor.View.Wpf;

public partial class PreviewWindow : MetroWindow
{
    private readonly PreviewViewModel _viewModel = new();
    private readonly ICircuitElement _circuit;
    
    
    public PreviewWindow(ICircuitElement circuit)
    {
        _circuit = circuit;
        DataContext = _viewModel;
        
        _viewModel.Editor.Initialized += Editor_OnInitialized;
        
        InitializeComponent();
    }
    
    ~PreviewWindow()
    {
        _viewModel.Editor.Initialized -= Editor_OnInitialized;
    }

    private void Editor_OnInitialized()
    {
        _viewModel.SpawnCircuit(_circuit);
    }
}