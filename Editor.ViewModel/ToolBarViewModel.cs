using CommunityToolkit.Mvvm.Input;
using Editor.Component;

namespace Editor.ViewModel;


public interface IPreview;


public partial class ToolBarViewModel : ViewModel
{
    public ToolBarItemViewModel[] Items { get; set; } = Array.Empty<ToolBarItemViewModel>();
    public ToolBarItemViewModel? SelectedItem { get; set; }
    
    
    public delegate void ItemSelectedHandler(ToolBarViewModel sender, ToolBarItemViewModel? item);
    public event ItemSelectedHandler? ItemSelected;


    [RelayCommand]
    private void SelectItem(ToolBarItemViewModel item)
    {
        ItemSelected?.Invoke(this, item);
    }
}


public class ToolBarItemViewModel : ViewModel
{
    public char Hotkey { get; set; }
    public string? Label => Hotkey.ToString();
    public IPreview Preview { get; set; } = default!;
    public IEntityBuilderFactory Factory { get; set; } = default!;
}