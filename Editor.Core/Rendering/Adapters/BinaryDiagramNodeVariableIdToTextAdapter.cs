using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Components.BinaryDiagrams;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Rendering.Adapters;

public class BinaryDiagramNodeVariableIdToTextAdapter : EditorComponentBase
{
    private BinaryDiagramNode _nodeComponent = default!;
    private LabeledShapeRenderer _rendererComponent = default!;
    
    protected override void OnInit()
    {
        _nodeComponent = Entity.GetRequiredComponent<BinaryDiagramNode>()!;
        _rendererComponent = Entity.GetRequiredComponent<LabeledShapeRenderer>()!;
        
        _nodeComponent.PropertyChanged += NodeComponent_OnPropertyChanged;
        NodeComponent_OnPropertyChanged(this, new PropertyChangedEventArgs(null));
    }
    
    protected override void OnDestroy()
    {
        _nodeComponent.PropertyChanged -= NodeComponent_OnPropertyChanged;
    }
    
    private void NodeComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _rendererComponent.Text = $"x{_nodeComponent.VariableId}";
    }
}