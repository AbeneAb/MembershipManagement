using Membership.API.Infrastrucuture.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Membership.API.Infrastrucuture
{
    public class MembershipContext : DbContext
    {
        public MembershipContext(DbContextOptions<MembershipContext> options):base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfiguration(new MemeberEntityTypeConfguration());
            modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthInformationEntityTypeConfiguration());
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<HealthInformation> HealthInformation { get; set; }
    }
    public class TransactionContextDesignFactory : IDesignTimeDbContextFactory<MembershipContext>
    {
        public MembershipContext CreateDbContext(string[] args)
        {
            var builderOptions = new DbContextOptionsBuilder<MembershipContext>().
               UseSqlServer("Server=localhost,5433;Initial Catalog=memberDb;User Id=sa;Password=Thynk1234;");
            return new MembershipContext(builderOptions.Options);
        }

    }
}
