using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.DTOs.Rental;
using MotorcycleRental.Application.Queries.Rental;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.Text.Json;

namespace MotorcycleRental.Application.Handlers.Rental
{
    public class GetRentalByIdHandler : IRequestHandler<GetRentalByIdQuery, GetRentalDto>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IPlanRepository _planRepository;
        private readonly ILogger<GetRentalByIdHandler> _logger;

        public GetRentalByIdHandler(
            IRentalRepository rentalRepository,
            IPlanRepository planRepository,
            ILogger<GetRentalByIdHandler> logger)
        {
            _rentalRepository = rentalRepository;
            _planRepository = planRepository;
            _logger = logger;
        }

        public async Task<GetRentalDto> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando aluguel por Id: {@Request}", request);

            var rental = await _rentalRepository.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException($"Aluguel com Id {request.Id} não encontrado.");

            var plan = await _planRepository.GetByDaysAsync(rental.Plan)
                       ?? throw new KeyNotFoundException($"Plano com {rental.Plan} dias não encontrado.");

            return new GetRentalDto(
                rental.InternalId,
                plan.Price,
                rental.TotalValue,
                rental.DeliveryManId,
                rental.MotorcycleId,
                rental.StartDate,
                rental.EndDate,
                rental.ExpectedEndDate,
                rental.DevolutionDate
            );
        }
    }

}
