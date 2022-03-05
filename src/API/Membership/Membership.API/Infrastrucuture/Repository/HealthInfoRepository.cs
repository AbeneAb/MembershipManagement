namespace Membership.API.Infrastrucuture.Repository
{
    public class HealthInfoRepository : IHealthRepository
    {
        private readonly MembershipContext _membershipContext;
        public HealthInfoRepository(MembershipContext membershipContext)
        {
            _membershipContext = membershipContext;
        }

        public async Task<Guid> CreateHealthData(HealthInformation healthInformation)
        {
            try
            {
                var data = await _membershipContext.HealthInformation.ToListAsync();
                await _membershipContext.HealthInformation.AddAsync(healthInformation);
                var result = await _membershipContext.SaveChangesAsync();
                return healthInformation.Id;
            }
            catch (Exception e)
            {

                throw;
            }
          
        }

        public async Task<IEnumerable<HealthInformation>> GetAll()
        {
            var healthInformation = _membershipContext.HealthInformation.AsNoTracking().ToListAsync();
            return await healthInformation;
        }

    }
}
