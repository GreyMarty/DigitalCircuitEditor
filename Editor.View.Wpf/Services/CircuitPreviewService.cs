using System.Windows;
using Editor.DecisionDiagrams.Circuits;
using Editor.ViewModel.Services;

namespace Editor.View.Wpf.Services;

public class CircuitPreviewService(Window parent) : ICircuitPreviewService
{
    private readonly Window _parent = parent;
    
    
    public void Show(ICircuitElement circuit)
    {
        var previewWindow = new PreviewWindow(circuit)
        {
            Owner = _parent
        };
        
        previewWindow.Show();
    }
}