using Editor.DecisionDiagrams.Circuits;

namespace Editor.ViewModel.Services;

public interface ICircuitPreviewService
{
    public void Show(ICircuitElement circuit);
}