using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Commands.Motorcycle;
using MotorcycleRental.Application.DTOs.Motorcycle;
using MotorcycleRental.Application.Events.Motorcycle;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Handlers.Motorcycle
{
    public class CreateMotorcycleHandler : IRequestHandler<CreateMotorcycleCommand, MotorcycleDto>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateMotorcycleHandler> _logger;

        public CreateMotorcycleHandler(IMotorcycleRepository repository, IMediator mediator, ILogger<CreateMotorcycleHandler> logger)
        {
            _repository = repository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<MotorcycleDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Criando moto: {@Request}", request);

            await Validate(request);

            var motorcycle = new MotorcycleEntity
            {
                Id = request.Id,
                Year = request.Year,
                Model = request.Model,
                Plate = request.Plate
            };

            var motorcycleAdd = await _repository.AddAsync(motorcycle);

            _logger.LogInformation("Moto cadastrada com sucesso: {Plate}", motorcycle.Plate);

            await _mediator.Publish(
                new MotorcycleCreatedEvent(motorcycleAdd.InternalId, motorcycle.Id, motorcycle.Plate, motorcycle.Model, motorcycle.Year),
                cancellationToken
            );

            return new MotorcycleDto(
                motorcycleAdd.Id,
                motorcycleAdd.Year,
                motorcycleAdd.Model,
                motorcycleAdd.Plate
            );
        }

        private async Task Validate(CreateMotorcycleCommand request)
        {
            if (request.Plate.Length > 10)
            {
                _logger.LogWarning("Placa inválida: {Plate}", request.Plate);
                throw new ValidationException("Placa inválida.");
            }

            if (await _repository.ExistsByPlateAsync(request.Plate))
            {
                _logger.LogWarning("Placa já cadastrada: {Plate}", request.Plate);
                throw new ValidationException("Placa já cadastrada.");
            }

            if (await _repository.GetByIdAsync(request.Id) != null)
            {
                _logger.LogWarning("ID já cadastrado: {Id}", request.Id);
                throw new ValidationException("ID já cadastrado.");
            }
        }

    }
}
