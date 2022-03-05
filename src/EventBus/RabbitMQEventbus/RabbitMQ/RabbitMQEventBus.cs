using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitMQEventbus.RabbitMQ;

public class RabbitMQEventBus : IEventBus, IDisposable
{
    const string BROKER_NAME = "EVENT_BUS";
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly ILogger<RabbitMQEventBus> _logger;
    private IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
    private readonly int _retryCount;
    private IModel _consumerChannel;
    private string _queueName;
    private readonly ILifetimeScope _autofac;

    public RabbitMQEventBus(IRabbitMQPersistentConnection persistentConnection, 
        ILogger<RabbitMQEventBus> logger,IEventBusSubscriptionsManager subManager,
        ILifetimeScope serviceProvider,
        string queueName = null,int retryCount =5)
    {
        _persistentConnection = persistentConnection;
        _logger = logger;
        _eventBusSubscriptionsManager = subManager;
        _queueName = queueName;
        _retryCount = retryCount;
        _consumerChannel = CreateConsumerChannel();
        _eventBusSubscriptionsManager.OnEventRemoved += _eventBusSubscriptionsManager_OnEventRemoved;
        _autofac = serviceProvider;
    }

    private void _eventBusSubscriptionsManager_OnEventRemoved(object? sender, string e)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using (var channel = _persistentConnection.CreateModel())
        {
            channel.QueueUnbind(queue: _queueName,
                exchange: BROKER_NAME,
                routingKey: e);

            if (_eventBusSubscriptionsManager.IsEmpty)
            {
                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }
    }

    private IModel CreateConsumerChannel() 
    {
        if (!_persistentConnection.IsConnected) { _persistentConnection.TryConnect(); }

        _logger.LogTrace("Creating RabbitMQ consumer channel");

        var channel = _persistentConnection.CreateModel();

        channel.ExchangeDeclare(BROKER_NAME, "direct");

        channel.QueueDeclare(queue: _queueName,durable:true,
            exclusive:false,autoDelete:false,arguments:null);

        channel.CallbackException += (sender, e) =>
        {
            _logger.LogWarning(e.Exception, "Recreating RabbitMQ consumer channel");
            _consumerChannel.Dispose();
            _consumerChannel = CreateConsumerChannel();

        };
        return channel;
    }
    private void StartBasicConsume() 
    {
        _logger.LogTrace("Starting basic consume");
        if(_consumerChannel != null) 
        {
            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
            consumer.Received += Consumer_Received;
            _consumerChannel.BasicConsume(
                queue: _queueName, 
                autoAck: false, 
                consumer: consumer);
        }
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey;
        var message = Encoding.UTF8.GetString(@event.Body.Span);
        try 
        { 
            await ProcessEvent(eventName, message);
        }
        catch(Exception ex) {
            _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
        }
        _consumerChannel.BasicAck(@event.DeliveryTag, multiple: false);
    }
    public async Task ProcessEvent(string eventName, string message) 
    {
        _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
        if (_eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName))
        {
            using (var scope = _autofac.BeginLifetimeScope())
            {
                var subscriptions = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler == null) continue;
                    var eventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
                    var @event = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });

                }
            }

        }
        else
        {
            _logger.LogWarning("No subscription for RabbitMQ event : {EventName}", eventName);
        }
    }

    public void Dispose()
    {
        if(_consumerChannel != null) _consumerChannel.Dispose();
        _eventBusSubscriptionsManager.Clear();
    }

    public void Publish(Event message)
    {
        if (!_persistentConnection.IsConnected) { _persistentConnection.TryConnect(); }
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>().WaitAndRetry(_retryCount, retryInterval => TimeSpan.FromSeconds(Math.Pow(2, retryInterval)),(ex,time) =>
            {
                _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", message.Id, $"{time.TotalSeconds:n1}", ex.Message);
            });

        var eventName = message.GetType().Name;
        _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", message.Id, eventName);
        using(var channel = _persistentConnection.CreateModel())
        {
            _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", message.Id);
            channel.ExchangeDeclare(exchange: BROKER_NAME, type: ExchangeType.Direct);
            var body = JsonSerializer.SerializeToUtf8Bytes(message,message.GetType(), new JsonSerializerOptions { WriteIndented = true});
            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                channel.BasicPublish(exchange: BROKER_NAME,
                    routingKey: eventName, mandatory: true,
                    basicProperties: null, body: body);
            });
        }


    }

    public void Subscribe<T, TH>()
        where T : Event
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _eventBusSubscriptionsManager.GetEventKey<T>();
        DoInternalSubscription(eventName);
        _eventBusSubscriptionsManager.AddSubscription<T, TH>();
        StartBasicConsume();
    }
    private void DoInternalSubscription(string eventName) 
    {
        var containsKey = _eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName);
        if (!containsKey) 
        {
            if (!_persistentConnection.IsConnected) 
            {
                _persistentConnection.TryConnect();
            }
            _consumerChannel.QueueBind(queue:_queueName,
                exchange:BROKER_NAME,
                routingKey:eventName);
        }
    }

    public void Unsubscribe<T, TH>()
        where T : Event
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _eventBusSubscriptionsManager.GetEventKey<T>();
        _logger.LogWarning("Unsubscribing from event {EventName}", eventName);
        _eventBusSubscriptionsManager.RemoveSubscription<T, TH>();
    }
}

