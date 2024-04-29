namespace Editor.Core.Wpf.ViewModel;

public class EditorEntitiesListViewModel
{
    public EditorEntitiesListItemViewModel[] Items { get; set; } = default!;

    public EditorEntitiesListItemViewModel? SelectedItem { get; set; }
}