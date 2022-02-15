using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Membership.API.Infrastrucuture.EntityConfiguration
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.Id).IsUnique();
            builder.HasOne(t=>t.Member).WithMany(m=>m.Transactions).HasForeignKey(t=>t.MemberId);

        }
    }
}
