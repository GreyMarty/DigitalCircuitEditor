using System.Numerics;
using Editor.Component;

namespace Editor.Core.Wpf.ViewModel;

public class EditorEntitiesListItemViewModel
{
    public EditorEntitiesListItemViewModel(string image, Func<Vector2?, IEntityBuilder> createBuilder, string? label = null)
    {
        Image = image;
        CreateBuilder = createBuilder;
    }
    
    
    public string? Label { get; set; }
    public string Image { get; set; }
    public Func<Vector2?, IEntityBuilder> CreateBuilder { get; set; }
}