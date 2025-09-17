using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Commands.Motorcycle;
using MotorcycleRental.Infrastructure.Interfaces;

namespace MotorcycleRental.Application.Handlers.Motorcycle
{
    public class UpdateMotorcycleHandler : IRequestHandler<UpdateMotorcycleCommand>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly ILogger<UpdateMotorcycleHandler> _logger;

        public UpdateMotorcycleHandler(IMotorcycleRepository repository, ILogger<UpdateMotorcycleHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Atualizando moto: {@Request}", request);

            var motorcycle = await _repository.GetByIdAsync(request.Id);
            if (motorcycle == null)
            {
                _logger.LogWarning("Moto com Id {Id} não encontrada", request.Id);
                throw new KeyNotFoundException($"Moto com Id {request.Id} não encontrada.");
            }

            motorcycle.Plate = request.Plate;
            await _repository.UpdateAsync(motorcycle);

            _logger.LogInformation("Moto com Id {Id} atualizada com sucesso", request.Id);
        }
    }

}
