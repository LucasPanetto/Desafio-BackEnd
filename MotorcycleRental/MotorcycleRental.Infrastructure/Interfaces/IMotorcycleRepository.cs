using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task<MotorcycleEntity> AddAsync(MotorcycleEntity motorcycle);
        Task<MotorcycleEntity?> GetByPlateAsync(string plate);
        Task<MotorcycleEntity?> GetByIdAsync(string id);
        Task<bool> ExistsByPlateAsync(string plate);
        Task<IEnumerable<MotorcycleEntity>> GetAllAsync();
        Task UpdateAsync(MotorcycleEntity motorcycle);
        Task DeleteAsync(MotorcycleEntity motorcycle);
    }
}
