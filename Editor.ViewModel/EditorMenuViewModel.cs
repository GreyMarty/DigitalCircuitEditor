using CommunityToolkit.Mvvm.Input;
using Editor.DecisionDiagrams.Operations;

namespace Editor.ViewModel;

public partial class EditorMenuViewModel : ViewModel
{
    public EditorMenuViewModel()
    {
        Operations =
        [
            new OperationViewModel
            {
                Label = "OR",
                Operation = new Or(),
            },
            new OperationViewModel
            {
                Label = "AND",
                Operation = new And(),
            },
        ];
    }
    
    
    public OperationViewModel[] Operations { get; set; }

    public IRelayCommand? ReduceCommand { get; set; } 
    public IRelayCommand<IBooleanOperation>? ApplyOperationCommand { get; set; }
    public IRelayCommand? ConvertCommand { get; set; }
}

public class OperationViewModel : ViewModel
{
    public string Label { get; set; } = default!;
    public IBooleanOperation Operation { get; set; } = default!;
}