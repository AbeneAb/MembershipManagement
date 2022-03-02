namespace Membership.API.Interfaces
{
    public interface IHealthRepository
    {
        Task<IEnumerable<HealthInformation>> GetAll();
        Task<Guid>  CreateHealthData(HealthInformation healthInformation);

    }
}
