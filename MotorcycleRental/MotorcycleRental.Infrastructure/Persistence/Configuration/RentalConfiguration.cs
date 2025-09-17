using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorcycleRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Persistence.Configuration
{
    public class RentalEntityConfiguration : IEntityTypeConfiguration<RentalEntity>
    {
        public void Configure(EntityTypeBuilder<RentalEntity> builder)
        {

            builder.HasKey(r => r.InternalId);
            builder.Property(r => r.InternalId)
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.DeliveryManId)
                   .IsRequired()
                   .HasMaxLength(50); 
            builder.Property(r => r.MotorcycleId)
                   .IsRequired();

            builder.Property(r => r.StartDate)
                   .IsRequired();

            builder.Property(r => r.EndDate)
                   .IsRequired();

            builder.Property(r => r.ExpectedEndDate)
                   .IsRequired();

            builder.Property(r => r.Plan)
                   .IsRequired()
                   .HasMaxLength(20); 
        }
    }
}
