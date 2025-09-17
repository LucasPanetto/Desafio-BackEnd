using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Persistence;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly MotorcycleRentalDbContext _context;

        public PlanRepository(MotorcycleRentalDbContext context)
        {
            _context = context;
        }

        public async Task<PlanEntity?> GetByDaysAsync(int days)
        {
            return await _context.Plans.FirstOrDefaultAsync(m => m.Days == days);
        }
    }
}
