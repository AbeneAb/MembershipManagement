
namespace Membership.API.Infrastrucuture
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MembershipContext>(options => options.UseSqlServer(configuration.GetConnectionString("MembershipConnectionString")));
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped< IHealthRepository ,HealthInfoRepository>();
          
            return services;
        }
    }
}
