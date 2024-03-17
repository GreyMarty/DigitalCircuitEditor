using TinyMessenger;

namespace Editor.Component.Events;

public class EntityInstantiated(object sender, Entity entity) : TinyMessageBase(sender)
{
    public Entity Entity { get; } = entity;
}