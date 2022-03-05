
namespace Membership.API.Handlers;

public class NewPulseDataHandler : IIntegrationEventHandler<NewPulseDataPosted>
{
    private readonly IHealthRepository _healthRepository;
    private readonly ILogger<NewPulseDataHandler> _logger;
    private IHubContext<NotificationsHub> _hubContext;
    public NewPulseDataHandler(IHealthRepository healthRepository,
        ILogger<NewPulseDataHandler> logger, IHubContext<NotificationsHub> hubContext)
    {
        _healthRepository = healthRepository;
        _logger = logger;
        _hubContext = hubContext;
    }
    public async Task Handle(NewPulseDataPosted @event)
    {
        if (@event.Id != Guid.Empty)
        {
            _logger.LogInformation("Handling integration event : {EventId} - {IntegrationEvent}", @event.Id, @event);
            await _healthRepository.CreateHealthData(new HealthInformation(@event.Pulse, @event.Systolic, @event.Diastolic, @event.Time, @event.DeviceId));
            await _hubContext.Clients.Group("HEALTH_DATA").SendAsync("newpulsedata", JsonSerializer.Serialize(@event));
            _logger.LogInformation("Create Health Data suceeded - RequestId: {RequestId}", @event.Id);
        }
        else
        {
            _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
        }
    }
}
