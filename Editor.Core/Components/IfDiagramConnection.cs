using PropertyChanged;

namespace Editor.Core.Components;

public class IfDiagramConnection : Connection
{
    public IfDiagramConnectionType Type { get; set; }

    [DependsOn(nameof(Type))]
    public override string? Label
    {
        get => Type.ToString();
        set { }
    }
}

public enum IfDiagramConnectionType
{
    Direct,
    If,
    True,
    False
}