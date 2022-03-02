
namespace RabbitMQEventbus.Abstractions
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        void AddSubscription<T, TH>()
            where T : Event
            where TH : IIntegrationEventHandler<T>;
        void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : Event;
        bool HasSubscriptionsForEvent<T>() where T : Event;
        bool HasSubscriptionsForEvent(string eventName);
        string GetEventKey<T>();
        Type GetEventTypeByName(string eventName);
        IEnumerable<EventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent<T>() where T : Event;
        IEnumerable<EventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent(string eventName);
        void Clear();
    }
}
