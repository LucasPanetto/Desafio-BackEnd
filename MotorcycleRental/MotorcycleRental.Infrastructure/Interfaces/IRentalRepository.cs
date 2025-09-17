using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Interfaces
{
    public interface IRentalRepository
    {
        Task<RentalEntity> AddAsync(RentalEntity deliverMan);
        Task<RentalEntity> UpdateAsync(RentalEntity deliverMan);
        Task<RentalEntity?> GetByIdAsync(int id);
        Task<RentalEntity?> GetByMotorcycleIdAsync(string motorcycleId);
    }
}
