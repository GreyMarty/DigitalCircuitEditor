using Editor.Core.Prefabs;
using Editor.Core.Wpf.Resources;

namespace Editor.Core.Wpf.ViewModel.DesignTime;

public class EditorEntitiesListDesignTimeViewModel : EditorEntitiesListViewModel
{
    public EditorEntitiesListDesignTimeViewModel()
    {
        Items = [
            new EditorEntitiesListItemViewModel(
                Images.Ifd,
                new IfDiagramNodeSpawnerFactory(),
                "If Diagram Node"
            )
        ];
    }
}