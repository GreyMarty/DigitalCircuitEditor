using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Prefabs;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Spawners;
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
            ),
            new EditorEntitiesListItemViewModel(
                Images.Const0,
                new InstantSpawnerFactory<SimpleSpawner<ConstNodeFactory<BinaryDiagramConnectionType>>>(),
                "Const node"
            ),
            new EditorEntitiesListItemViewModel(
                Images.Const0,
                new InstantSpawnerFactory<BinaryDiagramOutputNodeSpawner>(),
                "Output"
            )
        ];
    }
}