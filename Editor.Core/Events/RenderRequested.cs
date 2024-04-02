using TinyMessenger;

namespace Editor.Core.Events;

public record RenderRequested(object Sender) : ITinyMessage;