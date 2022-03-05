using Microsoft.Data.SqlClient;

namespace Membership.API.Extenstion
{
    public static class HostExtensions
    {
        public static IWebHost MigrateDatabase<TContext>(this IWebHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext 
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    logger.LogInformation("Migration database ...");
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation("Migration was a success");

                }
                catch (SqlException ex)
                {
                    logger?.LogError(ex, "An error ocured while migrating the db");
                    throw;
                }
            }
            return host;
        }
        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
