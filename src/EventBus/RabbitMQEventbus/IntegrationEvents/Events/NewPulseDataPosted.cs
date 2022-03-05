namespace RabbitMQEventbus.IntegrationEvents.Events;

public record NewPulseDataPosted : Event
{
    public string DeviceId { get; private init; }
    public uint Pulse { get; private init; }
    public uint Systolic { get; private init; }
    public uint Diastolic { get; private init; }
    public DateTime Time { get; private init; }
    public NewPulseDataPosted(string deviceId, uint pulse, uint systolic, uint diastolic)
    {
        DeviceId = deviceId;
        Pulse = pulse;
        Systolic = systolic;
        Diastolic = diastolic;
        Time = DateTime.UtcNow;
    }

}

