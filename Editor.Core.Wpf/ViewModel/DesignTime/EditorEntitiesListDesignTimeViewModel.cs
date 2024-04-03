using System.Windows.Media;
using System.Windows.Media.Imaging;
using Editor.Core.Components.Behaviors;
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
                x => IfDiagramNodeSpawnerPrefab.CreateBuilder(x)
                    .AddComponent<SpawnOnInitBehavior>(),
                "If Diagram Node"
            )
        ];
    }
}