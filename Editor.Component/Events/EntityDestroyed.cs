using TinyMessenger;

namespace Editor.Component.Events;

public class EntityDestroyed(object sender, IEntity entity) : TinyMessageBase(sender)
{
    public IEntity Entity { get; } = entity;
}

public class EntityDestroying(object sender, IEntity entity) : TinyMessageBase(sender)
{
    public IEntity Entity { get; } = entity;
}