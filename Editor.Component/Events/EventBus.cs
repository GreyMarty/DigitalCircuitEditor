using TinyMessenger;

namespace Editor.Component.Events;

public interface IEventBus
{
    public IEventBusSubscriber Subscribe(Func<bool>? globalFilter = null);
    public void Publish<TEvent>(TEvent eventArgs) where TEvent : class, ITinyMessage;
}

public interface IEventBusSubscriber
{
    public void Subscribe<TEvent>(Action<TEvent> handler, bool ignoreGlobalFilter = false) where TEvent : class, ITinyMessage;
    public void Unsubscribe<TEvent>() where TEvent : class, ITinyMessage;
}

public class EventBus(ITinyMessengerHub hub) : IEventBus
{
    private readonly ITinyMessengerHub _hub = hub;
    private readonly List<IEventBusSubscriber> _subscribers = [];
    

    public IEventBusSubscriber Subscribe(Func<bool>? globalFilter = null)
    {
        return new EventBusSubscriber(_hub, globalFilter);
    }

    public void Publish<TEvent>(TEvent eventArgs) where TEvent : class, ITinyMessage
    {
        _hub.Publish(eventArgs);
    }
}

internal class EventBusSubscriber(ITinyMessengerHub hub, Func<bool>? globalFilter = null) : IEventBusSubscriber
{
    private readonly ITinyMessengerHub _hub = hub;
    private readonly Func<bool>? _globalFilter = globalFilter;
    private readonly Dictionary<Type, TinyMessageSubscriptionToken> _subscriptions = [];
    
    
    public void Subscribe<TEvent>(Action<TEvent> handler, bool ignoreGlobalFilter = false) where TEvent : class, ITinyMessage
    {
        var token = _globalFilter is null || ignoreGlobalFilter
            ? _hub.Subscribe(handler)
            : _hub.Subscribe(handler, _ => _globalFilter());
        _subscriptions.Add(typeof(TEvent), token);
    }

    public void Unsubscribe<TEvent>() where TEvent : class, ITinyMessage
    {
        var token = _subscriptions[typeof(TEvent)];
        _hub.Unsubscribe<TEvent>(token);
    }
}