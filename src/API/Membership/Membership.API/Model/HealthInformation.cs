namespace Membership.API.Model
{
    public class HealthInformation
    {
        public Guid Id { get; set; }  
        public uint HeartRate { get; set; }
        public uint Systolic { get; set; }
        public uint Diastolic { get; set; }
        public DateTime Time { get; set; }
        public HealthInformation()
        {
            Id = Guid.NewGuid();
        }
    }
}
