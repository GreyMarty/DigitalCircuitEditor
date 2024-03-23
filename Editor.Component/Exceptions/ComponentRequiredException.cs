namespace Editor.Component.Exceptions;

public class ComponentRequiredException(Type type) : Exception
{
    public Type Type { get; } = type;
}