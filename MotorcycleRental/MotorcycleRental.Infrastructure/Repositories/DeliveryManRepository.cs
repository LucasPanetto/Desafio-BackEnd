using Microsoft.EntityFrameworkCore;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Persistence;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class DeliveryManRepository : IDeliveryManRepository
    {
        private readonly MotorcycleRentalDbContext _context;

        public DeliveryManRepository(MotorcycleRentalDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryManEntity> AddAsync(DeliveryManEntity deliveryMan)
        {
            _context.DeliveryMan.Add(deliveryMan);
            await _context.SaveChangesAsync();
            return deliveryMan;
        }

        public async Task<bool> ExistsByCnpjAsync(string cnpj)
        {
            return await _context.DeliveryMan.AnyAsync(d => d.Cnpj == cnpj);
        }

        public async Task<bool> ExistsByCnhNumberAsync(string cnhNumber)
        {
            return await _context.DeliveryMan.AnyAsync(d => d.CnhNumber == cnhNumber);
        }

        public async Task<DeliveryManEntity?> GetByIdAsync(string id)
        {
            return await _context.DeliveryMan
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DeliveryManEntity> UpdateAsync(DeliveryManEntity deliveryMan)
        {
            _context.DeliveryMan.Update(deliveryMan);
            await _context.SaveChangesAsync();
            return deliveryMan;
        }

    }
}
