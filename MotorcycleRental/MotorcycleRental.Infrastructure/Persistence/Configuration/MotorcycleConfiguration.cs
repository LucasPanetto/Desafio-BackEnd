using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Persistence.Configuration
{
    public class MotorcycleConfiguration : IEntityTypeConfiguration<MotorcycleEntity>
    {
        public void Configure(EntityTypeBuilder<MotorcycleEntity> builder)
        {

            builder.HasKey(m => m.InternalId);

            builder.Property(m => m.InternalId)
                   .ValueGeneratedOnAdd(); 

            builder.Property(m => m.Id)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(m => m.Id)
                   .IsUnique();

            builder.Property(m => m.Year)
                   .IsRequired();

            builder.Property(m => m.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.Plate)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.HasIndex(m => m.Plate)
                   .IsUnique();
        }
    }
}
