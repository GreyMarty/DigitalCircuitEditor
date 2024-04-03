using System.Numerics;
using Editor.Component;

namespace Editor.Core.Wpf.ViewModel;

public class EditorEntitiesListItemViewModel
{
    public EditorEntitiesListItemViewModel(string image, IEntityBuilderFactory factory, string? label = null)
    {
        Image = image;
        Factory = factory;
    }
    
    
    public string? Label { get; set; }
    public string Image { get; set; }
    public IEntityBuilderFactory Factory { get; set; }
}