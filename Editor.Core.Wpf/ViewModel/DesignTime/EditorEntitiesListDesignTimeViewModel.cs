using Editor.Core.Prefabs.Factories.Previews;
using Editor.Core.Wpf.Resources;

namespace Editor.Core.Wpf.ViewModel.DesignTime;

public class EditorEntitiesListDesignTimeViewModel : EditorEntitiesListViewModel
{
    public EditorEntitiesListDesignTimeViewModel()
    {
        Items = [
            new EditorEntitiesListItemViewModel(
                Images.Ifd,
                new BinaryDiagramNodePreviewFactory(),
                "If Diagram Node"
            ),
            new EditorEntitiesListItemViewModel(
                Images.Const0,
                new ConstNodePreviewFactory(),
                "Const node"
            ),
            new EditorEntitiesListItemViewModel(
                Images.Const0,
                new OutputNodePreviewFactory(),
                "Output"
            )
        ];
    }
}