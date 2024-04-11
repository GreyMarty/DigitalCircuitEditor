using TinyMessenger;

namespace Editor.Core.Events;

public record DestroyRequested(object Sender) : ITinyMessage;