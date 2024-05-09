using CommunityToolkit.Mvvm.Input;
using Editor.DecisionDiagrams.Operations;

namespace Editor.ViewModel;

public class EditorMenuViewModel : ViewModel
{
    public EditorMenuViewModel()
    {
        Operations =
        [
            new OperationViewModel
            {
                Label = "NOT",
                Operation = new Not()
            },
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
            new OperationViewModel
            {
                Label = "XOR",
                Operation = new Xor()
            }
        ];
    }


    public string FileName { get; set; } = "Untitled.yml";
    public OperationViewModel[] Operations { get; set; }

    public IRelayCommand? ReduceCommand { get; set; } 
    public IRelayCommand<IOperation>? ApplyOperationCommand { get; set; }
    public IRelayCommand? ConvertCommand { get; set; }
    public IRelayCommand? SaveCommand { get; set; }
    public IRelayCommand? SaveAsCommand { get; set; }
    public IRelayCommand? LoadCommand { get; set; }
}

public class OperationViewModel : ViewModel
{
    public string Label { get; set; } = default!;
    public IOperation Operation { get; set; } = default!;
}