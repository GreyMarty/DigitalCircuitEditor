using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Entities;

public class EntityTreeBuilder : IEntityTreeBuilder
{
    private readonly List<IEntityTreeBuilder> _children = [];


    public IEntityBuilder Node { get; init; } = Entity.CreateBuilder();
    

    public IEntityTreeBuilder AddNode(IEntityTreeBuilder? node = null)
    {
        _children.Add(node ?? new EntityTreeBuilder());
        return this;
    }

    public IEnumerable<IEntity> Build()
    {
        var entity = Node.Build();
        return [entity, ..BuildChildren(entity, _children)];
    }

    private static List<IEntity> BuildChildren(IEntity parent, IEnumerable<IEntityTreeBuilder> children)
    {
        var entities = new List<IEntity>();
        
        foreach (var child in children)
        {
            child.Node.AddComponent<ChildOf>(x => x.Parent = parent);
            entities.AddRange(child.Build());
        }

        return entities;
    }
}