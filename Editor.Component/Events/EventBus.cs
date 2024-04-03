using TinyMessenger;

namespace Editor.Component.Events;

public interface IEventBus
{
    public IEventBusSubscriber Subscribe();
    public void Publish<TEvent>(TEvent eventArgs) where TEvent : class, ITinyMessage;
}

public interface IEventBusSubscriber
{
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class, ITinyMessage;
    public void Unsubscribe<TEvent>() where TEvent : class, ITinyMessage;
    public void Publish<TEvent>(TEvent eventArgs) where TEvent : class, ITinyMessage;
}

public class EventBus(ITinyMessengerHub hub) : IEventBus
{
    private readonly ITinyMessengerHub _hub = hub;
    private readonly List<IEventBusSubscriber> _subscribers = [];
    

    public IEventBusSubscriber Subscribe()
    {
        return new EventBusSubscriber(_hub);
    }

    public void Publish<TEvent>(TEvent eventArgs) where TEvent : class, ITinyMessage
    {
        _hub.Publish(eventArgs);
    }
}

internal class EventBusSubscriber(ITinyMessengerHub hub) : IEventBusSubscriber
{
    private readonly ITinyMessengerHub _hub = hub;
    private readonly Dictionary<Type, TinyMessageSubscriptionToken> _subscriptions = [];
    
    
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class, ITinyMessage
    {
        var token = _hub.Subscribe(handler);
        _subscriptions.Add(typeof(TEvent), token);
    }

    public void Unsubscribe<TEvent>() where TEvent : class, ITinyMessage
    {
        var token = _subscriptions[typeof(TEvent)];
        _hub.Unsubscribe<TEvent>(token);
    }

    public void Publish<TEvent>(TEvent eventArgs) where TEvent : class, ITinyMessage
    {
        _hub.Publish(eventArgs);
    }
}