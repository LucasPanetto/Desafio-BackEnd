using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Consumer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Consumer.Data
{
    public class NotifyDbContext : DbContext
    {
        public NotifyDbContext(DbContextOptions<NotifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notify> Notify { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notify>(entity =>
            {
                entity.ToTable("Notify"); 
                entity.HasKey(e => e.Id);
            });
        }
    }
}
