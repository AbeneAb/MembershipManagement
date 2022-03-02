using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace RabbitMQEventbus.RabbitMQ;

public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQPersistentConnection> _logger;
    IConnection _connection;
    bool _disposed;
    private readonly int _retryCount;
    object sync_root = new object();

    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory,
        ILogger<RabbitMQPersistentConnection> logger,int retryCount = 5)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _retryCount = retryCount;
    }
    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    public IModel CreateModel()
    {
        if (!IsConnected) { throw new InvalidOperationException("No Rabbit connections are available to perform");
        }
        return _connection.CreateModel();
    }

    public void Dispose()
    {
        if(_disposed) { return; }
        _disposed = true;
        try
        {
            _connection.ConnectionShutdown -= OnConnectionShutdown;
            _connection.CallbackException -= OnCallbackException;
            _connection.ConnectionBlocked -= OnConnectionBlocked;
        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex.ToString());
        }

    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("Rabbit connection shutdown. Trying to reconnect...");
        TryConnect();
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ connection throw connection. Trying to re-connection...");
        TryConnect();
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ connection is on shutdown. Trying to reconnect...");
        TryConnect();
    }

    public bool TryConnect()
    {
        _logger.LogInformation("RabbitMQ Client is trying to reconnect");
        lock (sync_root) 
        {
            var policy = Policy.Handle<SocketException>().Or<BrokerUnreachableException>().WaitAndRetry(_retryCount, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)), (ex, time) =>
               {
                   _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
               });
            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });
            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;
                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);
                return true;
            }
            else
            {
                _logger.LogCritical("Fatal error: RabbitMQ Connections could not be created or opened");
                return false;
            }

        }

    }
}

