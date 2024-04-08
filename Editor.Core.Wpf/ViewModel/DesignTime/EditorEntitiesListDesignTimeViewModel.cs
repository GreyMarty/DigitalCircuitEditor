using Editor.Core.Prefabs;
using Editor.Core.Prefabs.IfDiagrams;
using Editor.Core.Spawners;
using Editor.Core.Wpf.Resources;

namespace Editor.Core.Wpf.ViewModel.DesignTime;

public class EditorEntitiesListDesignTimeViewModel : EditorEntitiesListViewModel
{
    public EditorEntitiesListDesignTimeViewModel()
    {
        Items = [
            new EditorEntitiesListItemViewModel(
                Images.Ifd,
                new InstantSpawnerFactory<BinaryDiagramNodeSpawner>(),
                "If Diagram Node"
            )
        ];
    }
}