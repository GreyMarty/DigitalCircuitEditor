using TinyMessenger;

namespace Editor.Core.Events;

public record RenderRequested(object Sender, bool Force = false) : ITinyMessage;