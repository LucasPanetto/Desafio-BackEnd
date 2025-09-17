using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Commands.Motorcycle;
using MotorcycleRental.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Handlers.Motorcycle
{
    public class DeleteMotorcycleHandler : IRequestHandler<DeleteMotorcycleCommand>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<DeleteMotorcycleHandler> _logger;

        public DeleteMotorcycleHandler(
            IMotorcycleRepository motorcycleRepository,
            IRentalRepository rentalRepository,
            ILogger<DeleteMotorcycleHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _rentalRepository = rentalRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deletando moto: {@Request}", request);

            var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
            if (motorcycle == null)
            {
                _logger.LogWarning("Moto com Id {Id} não encontrada", request.Id);
                throw new KeyNotFoundException($"Moto com Id {request.Id} não encontrada.");
            }

            var rental = await _rentalRepository.GetByMotorcycleIdAsync(request.Id);
            if (rental != null)
            {
                _logger.LogWarning("Moto com Id {Id} possui aluguel ativo", request.Id);
                throw new ValidationException("Moto possui aluguel ativo.");
            }

            await _motorcycleRepository.DeleteAsync(motorcycle);

            _logger.LogInformation("Moto com Id {Id} deletada com sucesso", request.Id);
        }
    }
}
