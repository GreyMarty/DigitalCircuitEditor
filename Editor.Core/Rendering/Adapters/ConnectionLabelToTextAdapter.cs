using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Rendering.Adapters;

public class ConnectionLabelToTextAdapter : EditorComponentBase
{
    private Connection _connectionComponent = default!;
    private LabeledShapeRenderer _rendererComponent = default!;


    protected override void OnInit()
    {
        _connectionComponent = Entity.GetRequiredComponent<Connection>()!;
        _rendererComponent = Entity.GetRequiredComponent<LabeledShapeRenderer>()!;
        
        _connectionComponent.PropertyChanged += ConnectionComponent_OnPropertyChanged;
        ConnectionComponent_OnPropertyChanged(this, new PropertyChangedEventArgs(null));
    }

    protected override void OnDestroy()
    {
        _connectionComponent.PropertyChanged -= ConnectionComponent_OnPropertyChanged;
    }

    private void ConnectionComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _rendererComponent.Text = _connectionComponent.Label;
    }
}