using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Interfaces
{
    public interface IDeliveryManRepository
    {
        Task<DeliveryManEntity> AddAsync(DeliveryManEntity deliverMan);
        Task<bool> ExistsByCnpjAsync(string cnpj);
        Task<bool> ExistsByCnhNumberAsync(string cnhNumber);
        Task<DeliveryManEntity?> GetByIdAsync(string id);
        Task<DeliveryManEntity> UpdateAsync(DeliveryManEntity deliveryMan);
    }
}
