namespace HealthData.API.BackgroundTask;

public class HealthDataMockService : IHostedService
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<HealthDataMockService> _logger;
    private Timer _timer;
    private int _executionCount =0;
    public HealthDataMockService(IEventBus eventBus, ILogger<HealthDataMockService> logger)
    {
        _eventBus = eventBus;   
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed hosted service starting");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;  
    }
    private void DoWork(object? obj)
    {
        _logger.LogInformation("Background service is triggered");
        Random deviceId = new Random();
        int deviceIdVal = deviceId.Next(1, 10);
        Random pulse = new Random();
        int pulseVal = pulse.Next(35, 140);
        Random systolic = new Random();
        int sysVal = systolic.Next(81, 160);
        Random diastolic = new Random();
        int diastolicVal = diastolic.Next(80, sysVal);

        var newPulseData = new NewPulseDataPosted(string.Format("Device {0}",deviceIdVal),Convert.ToUInt32(pulseVal),Convert.ToUInt32(sysVal), Convert.ToUInt32(diastolicVal));
        _eventBus.Publish(newPulseData);
        var count = Interlocked.Increment(ref _executionCount);
        _executionCount++;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed service is stopping");
        _timer?.Change(Timeout.Infinite,0);
        return  Task.CompletedTask;
    }
}

