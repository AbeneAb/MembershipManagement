
namespace Membership.API.Handlers;

public class NewPulseDataHandler : IIntegrationEventHandler<NewPulseDataPosted>
{
    private readonly IHealthRepository _healthRepository;
    private readonly ILogger<NewPulseDataHandler> _logger;
    public NewPulseDataHandler(IHealthRepository healthRepository,ILogger<NewPulseDataHandler> logger)
    {
        _healthRepository = healthRepository;
        _logger = logger;
    }
    public async Task Handle(NewPulseDataPosted @event)
    {
        if (@event.Id != Guid.Empty)
        {
            _logger.LogInformation("Handling integration event : {EventId} - {IntegrationEvent}", @event.Id, @event);
            var result = await _healthRepository.CreateHealthData(new HealthInformation(@event.Pulse, @event.Systolic, @event.Diastolic, @event.Time, @event.DeviceId));
            _logger.LogInformation("Create Health Data suceeded - RequestId: {RequestId}", @event.Id);
        }
        else
        {
            _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
        }
    }
}
