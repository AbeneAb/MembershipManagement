namespace RabbitMQEventbus.Abstractions;

public interface IEventBus
{
    void Publish(Event message);
    void Subscribe<T, TH>() where T : Event where TH : IIntegrationEventHandler<T>;
    void Unsubscribe<T, TH>() where T : Event where TH : IIntegrationEventHandler<T>;
}

