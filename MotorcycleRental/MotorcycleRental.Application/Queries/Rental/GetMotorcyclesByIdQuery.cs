using MediatR;
using MotorcycleRental.Application.DTOs.Rental;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries.Rental
{
    public record GetRentalByIdQuery(int Id) : IRequest<GetRentalDto>;

}
