namespace Editor.Component;

public interface IEntityTreeBuilder
{
    public IEntityBuilder Node { get; }
    
    public IEntityTreeBuilder AddNode(IEntityTreeBuilder? node = null);
    public IEnumerable<IEntity> Build();
}