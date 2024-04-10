using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Adapters;

public class NodeLabelToTextAdapter<TConnectionType> : EditorComponentBase where TConnectionType : notnull
{
    private Node<TConnectionType> _nodeComponent = default!;
    private LabeledShapeRenderer _rendererComponent = default!;
    
    protected override void OnInit()
    {
        _nodeComponent = Entity.GetRequiredComponent<Node<TConnectionType>>()!;
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
        _rendererComponent.Text = _nodeComponent.Label;
    }
}