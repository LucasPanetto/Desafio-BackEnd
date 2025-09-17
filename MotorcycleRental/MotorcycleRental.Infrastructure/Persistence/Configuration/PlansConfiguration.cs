using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Persistence.Configuration
{
    public class PlansEntityConfiguration : IEntityTypeConfiguration<PlanEntity>
    {
        public void Configure(EntityTypeBuilder<PlanEntity> builder)
        {
            builder.HasKey(p => p.InternalId);
            builder.Property(p => p.InternalId)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.Days)
                   .IsRequired();

            builder.Property(p => p.Price)
                   .IsRequired();

            builder.HasData(
                new PlanEntity { InternalId = 1, Days = 7, Price = 30.0 },
                new PlanEntity { InternalId = 2, Days = 15, Price = 28.0 },
                new PlanEntity { InternalId = 3, Days = 30, Price = 22.0 },
                new PlanEntity { InternalId = 4, Days = 45, Price = 20.0 },
                new PlanEntity { InternalId = 5, Days = 50, Price = 18.0 }
            );
        }
    }
}
