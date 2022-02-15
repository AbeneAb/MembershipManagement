using Membership.API.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Membership.API.Infrastrucuture.EntityConfiguration
{
    public class MemeberEntityTypeConfguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x=>x.FirstName).IsUnique(false).IsClustered(false);
            builder.HasIndex(x=>x.LastName).IsUnique(false).IsClustered(false);
            builder.HasIndex(x => x.Email).IsUnique(true).IsClustered(false);
            builder.HasMany(x => x.Transactions).WithOne();
        }
    }
}
