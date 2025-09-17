using MediatR;
using MotorcycleRental.Application.Commands.Rental;
using MotorcycleRental.Application.DTOs.Rental;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Handlers.DeliveryMan
{
    public class CloseRentalHandler : IRequestHandler<CloseRentalCommand, CloseRentalDto>
    {
        private readonly IDeliveryManRepository _repositoryDeliveryMan;
        private readonly IMotorcycleRepository _repositoryMotorcycle;
        private readonly IRentalRepository _rentalRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IMediator _mediator;

        public CloseRentalHandler(
            IDeliveryManRepository repositoryDeliveryMan,
            IMediator mediator,
            IMotorcycleRepository repositoryMotorcycle,
            IRentalRepository rentalRepository,
            IPlanRepository planRepository)
        {
            _repositoryDeliveryMan = repositoryDeliveryMan;
            _repositoryMotorcycle = repositoryMotorcycle;
            _mediator = mediator;
            _rentalRepository = rentalRepository;
            _planRepository = planRepository;
        }

        public async Task<CloseRentalDto> Handle(CloseRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetByIdAsync(request.InternalId)
                         ?? throw new KeyNotFoundException("Aluguel não encontrado.");

            await ValidateData(request, rental);

            var plan = await _planRepository.GetByDaysAsync(rental.Plan)
                       ?? throw new KeyNotFoundException("Plano não encontrado.");

            int daysRental = (request.DevolutionDate - rental.StartDate).Days + 1;
            double totalValue;

            if (request.DevolutionDate.Date < rental.ExpectedEndDate.Date)
            {
                int daysNotRental = (rental.ExpectedEndDate - request.DevolutionDate).Days;
                double fineRate = rental.Plan switch { 7 => 0.2, 15 => 0.4, _ => 0 };
                totalValue = (daysRental * plan.Price) + (daysNotRental * plan.Price * fineRate);
            }
            else if (request.DevolutionDate.Date > rental.ExpectedEndDate.Date)
            {
                int daysOverRental = (request.DevolutionDate - rental.ExpectedEndDate).Days;
                totalValue = (daysRental * plan.Price) + (daysOverRental * 50);
            }
            else
            {
                totalValue = daysRental * plan.Price;
            }

            rental.TotalValue = totalValue;
            rental.DevolutionDate = request.DevolutionDate;

            await _rentalRepository.UpdateAsync(rental);

            return new CloseRentalDto($"Data de devolução informada com sucesso. Valor: R${totalValue:F2}");
        }

        private Task ValidateData(CloseRentalCommand request, RentalEntity rental)
        {
            if (request.DevolutionDate.Date < rental.StartDate.Date)
                throw new ValidationException("Data de devolução não pode ser menor que a data de início.");

            if (rental.DevolutionDate != null)
                throw new ValidationException("Moto já devolvida.");

            return Task.CompletedTask;
        }
    }
}
