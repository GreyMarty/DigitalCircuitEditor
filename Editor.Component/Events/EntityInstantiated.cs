using TinyMessenger;

namespace Editor.Component.Events;

public class EntityInstantiated(object sender, IEntity entity) : TinyMessageBase(sender)
{
    public IEntity Entity { get; } = entity;
}