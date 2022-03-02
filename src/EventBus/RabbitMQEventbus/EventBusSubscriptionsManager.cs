namespace RabbitMQEventbus;

public partial class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public EventBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    public bool IsEmpty => _handlers is { Count: 0 };

    public event EventHandler<string> OnEventRemoved;

    public void AddSubscription<T, TH>()
        where T : Event
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        DoAddSubscription(eventName, typeof(TH));
        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }
    private void DoAddSubscription(string eventName, Type handler)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            _handlers.Add(eventName, new List<SubscriptionInfo>());
        }
        if (_handlers[eventName].Any(t => t.HandlerType == handler))
        {
            throw new ArgumentException(
             $"Handler Type {handler.Name} already registered for '{eventName}'", nameof(handler));
        }
        _handlers[eventName].Add(SubscriptionInfo.Typed(handler));
    }
    public bool HasSubscriptionsForEvent<T>() where T : Event
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    public void RemoveSubscription<T, TH>()
        where T : Event
        where TH : IIntegrationEventHandler<T>
    {
        var handlerToRemove = FindSubscriptionToRemove<T, TH>();
        var eventName = GetEventKey<T>();
        DoRemoveHandler(eventName, handlerToRemove);
    }
    private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
        if (subsToRemove != null)
        {
            _handlers[eventName].Remove(subsToRemove);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }
                RaiseOnEventRemoved(eventName);
            }

        }
    }
    private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
       where T : Event
       where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return null;
        }
        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }
    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }
    public string GetEventKey<T>()
    {
        return typeof(T).Name;
    }

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event
    {
        var key = GetEventKey<T>();
        return _handlers[key];
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
    {
        return _handlers[eventName];
    }

    public void Clear()
    {
        _handlers.Clear();
    }

    public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);
}

