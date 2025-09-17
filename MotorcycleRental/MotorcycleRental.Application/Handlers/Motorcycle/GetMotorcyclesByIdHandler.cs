using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Queries.Motorcycle;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MotorcycleRental.Application.Handlers.Motorcycle
{
    public class GetMotorcyclesByIdHandler : IRequestHandler<GetMotorcyclesByIdQuery, MotorcycleEntity>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly ILogger<GetMotorcyclesByIdHandler> _logger;

        public GetMotorcyclesByIdHandler(IMotorcycleRepository repository, ILogger<GetMotorcyclesByIdHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<MotorcycleEntity> Handle(GetMotorcyclesByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando moto por Id: {@Request}", request);

            if (string.IsNullOrEmpty(request.Id))
                throw new ValidationException("Id da moto não pode ser vazio.");

            var motorcycle = await _repository.GetByIdAsync(request.Id);
            if (motorcycle == null)
            {
                _logger.LogWarning("Moto com Id {Id} não encontrada", request.Id);
                throw new KeyNotFoundException($"Moto com Id {request.Id} não encontrada.");
            }

            return motorcycle;
        }
    }
}
