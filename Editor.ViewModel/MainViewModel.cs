using System.Numerics;
using Editor.Core.Components;

namespace Editor.ViewModel;

public class MainViewModel : ViewModel
{
    public MainViewModel()
    {
        ToolBar.ItemSelected += ToolBar_OnItemSelected;
    }

    ~MainViewModel()
    {
        ToolBar.ItemSelected -= ToolBar_OnItemSelected;
    }


    public ToolBarViewModel ToolBar { get; } = new();
    public EditorViewModel Editor { get; } = new();
    
    
    private void ToolBar_OnItemSelected(ToolBarViewModel sender, ToolBarItemViewModel? item)
    {
        if (item?.Factory is not { } factory)
        {
            return;
        }
        
        Editor.Context.Instantiate(factory.Create()
            .ConfigureComponent<Position>(x => x.Value = Vector2.One * 10000)
        );
    }
}