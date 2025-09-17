using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Persistence.Configuration;

namespace MotorcycleRental.Infrastructure.Persistence
{
    public class MotorcycleRentalDbContext : DbContext
    {
        public MotorcycleRentalDbContext(DbContextOptions<MotorcycleRentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<MotorcycleEntity> Motorcycles { get; set; }
        public DbSet<DeliveryManEntity> DeliveryMan { get; set; }
        public DbSet<RentalEntity> Rentals { get; set; }
        public DbSet<PlanEntity> Plans { get; set; }
        public DbSet<NotifyEntity> Notifys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MotorcycleConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryManConfiguration());
            modelBuilder.ApplyConfiguration(new RentalEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlansEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NotifyEntityConfiguration());

        }
    }
}
