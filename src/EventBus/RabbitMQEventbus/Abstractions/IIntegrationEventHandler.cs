namespace RabbitMQEventbus.Abstractions;

public interface IIntegrationEventHandler<in TEvent> : IIntegrationEventHandler where TEvent : Event
{
    Task Handle(TEvent @event);
}
public interface IIntegrationEventHandler
{
}

