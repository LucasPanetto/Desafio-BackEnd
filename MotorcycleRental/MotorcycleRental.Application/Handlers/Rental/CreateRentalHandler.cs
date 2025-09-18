using MediatR;
using MotorcycleRental.Application.Commands.Rental;
using MotorcycleRental.Application.DTOs.Rental;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Handlers.DeliveryMan
{
    public class CreateRentalHandler : IRequestHandler<CreateRentalCommand, CreateRentalDto>
    {
        private readonly IDeliveryManRepository _repositoryDeliveryMan;
        private readonly IMotorcycleRepository _repositoryMotorcycle;
        private readonly IRentalRepository _rentalRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IMediator _mediator;
        private static readonly List<string> ValidCategories = new() { "A", "AB" };

        public CreateRentalHandler(
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

        public async Task<CreateRentalDto> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);

            var rental = new RentalEntity
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ExpectedEndDate = request.ExpectedEndDate,
                DeliveryManId = request.DeliveryManId,
                MotorcycleId = request.MotorcycleId,
                Plan = request.Plan,
            };

            var rentalCreated = await _rentalRepository.AddAsync(rental);

            return new CreateRentalDto(
                rentalCreated.InternalId,
                rentalCreated.DeliveryManId,
                rentalCreated.MotorcycleId,
                rentalCreated.StartDate,
                rentalCreated.EndDate,
                rentalCreated.ExpectedEndDate,
                request.Plan
            );
        }

        private async Task ValidateData(CreateRentalCommand request)
        {
            var rental = await _rentalRepository.GetByMotorcycleIdAsync(request.MotorcycleId);
            if (rental != null)
            {
                throw new ValidationException("Moto ja alugada.");
            }

            var deliveryMan = await _repositoryDeliveryMan.GetByIdAsync(request.DeliveryManId)
                              ?? throw new KeyNotFoundException($"Entregador não encontrado: {request.DeliveryManId}.");

            if (!ValidCategories.Contains(deliveryMan.CnhType, StringComparer.OrdinalIgnoreCase))
                throw new ValidationException("Entregador não habilitado.");

            var motorcycle = await _repositoryMotorcycle.GetByIdAsync(request.MotorcycleId)
                             ?? throw new KeyNotFoundException($"Moto não encontrada: {request.MotorcycleId}.");

            if (request.StartDate.Date != DateTime.Today.AddDays(1))
                throw new ValidationException("A data de início da locação deve ser o primeiro dia após a data de criação.");

            var plan = await _planRepository.GetByDaysAsync(request.Plan)
                       ?? throw new ValidationException("Plano informado é inválido.");

            if (request.ExpectedEndDate.Date != DateTime.Today.AddDays(plan.Days + 1))
                throw new ValidationException("A data de previsão de término da locação deve ser o primeiro dia após a data de criação + o plano.");
        }
    }

}
