using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Membership.API.Infrastrucuture.EntityConfiguration
{
    public class HealthInformationEntityTypeConfiguration : IEntityTypeConfiguration<HealthInformation>
    {
        public void Configure(EntityTypeBuilder<HealthInformation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.Property(x=>x.Diastolic).IsRequired();
            builder.Property(x=>x.HeartRate).IsRequired();
            builder.Property(x=>x.Systolic).IsRequired();

        }
    }
}
