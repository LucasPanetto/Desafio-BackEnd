using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Queries.Motorcycle;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.Text.Json;

namespace MotorcycleRental.Application.Handlers.Motorcycle
{
    public class GetMotorcyclesHandler : IRequestHandler<GetMotorcyclesQuery, IEnumerable<MotorcycleEntity>>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly ILogger<GetMotorcyclesHandler> _logger;
        public GetMotorcyclesHandler(IMotorcycleRepository repository, ILogger<GetMotorcyclesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<MotorcycleEntity>> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(string.Format("GetMotorcyclesHandler -> {0}", JsonSerializer.Serialize(request)));

            if (!string.IsNullOrEmpty(request.Plate))
            {
                var motorcycle = await _repository.GetByPlateAsync(request.Plate);
                if (motorcycle == null)
                    return Enumerable.Empty<MotorcycleEntity>(); // ou lance DomainException se quiser

                return new List<MotorcycleEntity> { motorcycle };
            }

            return await _repository.GetAllAsync();
        }
    }
}
