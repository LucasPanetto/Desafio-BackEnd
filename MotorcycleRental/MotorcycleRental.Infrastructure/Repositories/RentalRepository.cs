using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Persistence;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly MotorcycleRentalDbContext _context;

        public RentalRepository(MotorcycleRentalDbContext context)
        {
            _context = context;
        }

        public async Task<RentalEntity> AddAsync(RentalEntity rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<RentalEntity> UpdateAsync(RentalEntity rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<RentalEntity?> GetByIdAsync(int id)
        {
            return await _context.Rentals.FirstOrDefaultAsync(m => m.InternalId == id);
        }

        public async Task<RentalEntity?> GetByMotorcycleIdAsync(string motorcycleId)
        {
            return await _context.Rentals.FirstOrDefaultAsync(m => m.MotorcycleId == motorcycleId);
        }
    }
}
