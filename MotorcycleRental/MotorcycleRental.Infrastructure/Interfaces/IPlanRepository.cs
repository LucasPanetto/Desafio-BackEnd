using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Interfaces
{
    public interface IPlanRepository
    {
        Task<PlanEntity?> GetByDaysAsync(int days);
    }
}
