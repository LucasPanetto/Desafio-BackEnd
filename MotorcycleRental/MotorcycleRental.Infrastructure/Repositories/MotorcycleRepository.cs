using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Persistence;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly MotorcycleRentalDbContext _context;

        public MotorcycleRepository(MotorcycleRentalDbContext context)
        {
            _context = context;
        }

        public async Task<MotorcycleEntity> AddAsync(MotorcycleEntity motorcycle)
        {
            _context.Motorcycles.Add(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle;
        }

        public async Task<MotorcycleEntity?> GetByPlateAsync(string plate)
        {
            return await _context.Motorcycles
                                 .FirstOrDefaultAsync(m => m.Plate == plate);
        }

        public async Task<bool> ExistsByPlateAsync(string plate)
        {
            return await _context.Motorcycles
                                 .AnyAsync(m => m.Plate == plate);
        }

        public async Task<IEnumerable<MotorcycleEntity>> GetAllAsync()
        {
            return await _context.Motorcycles.ToListAsync();
        }

        public async Task UpdateAsync(MotorcycleEntity motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task DeleteAsync(MotorcycleEntity motorcycle)
        {
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task<MotorcycleEntity?> GetByIdAsync(string id)
        {
            return await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
