using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Persistence.Configuration
{
    public class DeliveryManConfiguration : IEntityTypeConfiguration<DeliveryManEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryManEntity> builder)
        {
            builder.HasKey(d => d.InternalId);

            builder.Property(d => d.InternalId)
                   .ValueGeneratedOnAdd(); 

            builder.Property(d => d.Id)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(d => d.Id)
               .IsUnique();

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(d => d.Cnpj)
                   .IsRequired()
                   .HasMaxLength(20); 
            builder.HasIndex(d => d.Cnpj)
                   .IsUnique();

            builder.Property(d => d.Birthday)
                   .IsRequired();

            builder.Property(d => d.CnhNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(d => d.CnhNumber)
                   .IsUnique();

            builder.Property(d => d.CnhType)
                   .IsRequired()
                   .HasMaxLength(5); 

            builder.Property(d => d.CnhImagePath)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
