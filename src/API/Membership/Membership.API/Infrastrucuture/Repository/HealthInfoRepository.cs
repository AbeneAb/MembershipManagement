namespace Membership.API.Infrastrucuture.Repository
{
    public class HealthInfoRepository : IHealthRepository
    {
        private readonly MembershipContext _membershipContext;
        public HealthInfoRepository(MembershipContext membershipContext)
        {
            _membershipContext = membershipContext;
        }
        public async Task<IEnumerable<HealthInformation>> GetAll()
        {
            var healthInformation = _membershipContext.HealthInformation.AsNoTracking().ToListAsync();
            return await healthInformation;
        }
    }
}
