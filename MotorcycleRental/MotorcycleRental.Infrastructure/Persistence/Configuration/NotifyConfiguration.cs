using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Persistence.Configuration
{
    public class NotifyEntityConfiguration : IEntityTypeConfiguration<NotifyEntity>
    {
        public void Configure(EntityTypeBuilder<NotifyEntity> builder)
        {

            builder.HasKey(r => r.InternalId);
            builder.Property(r => r.InternalId)
                   .ValueGeneratedOnAdd();
        }
    }
}
