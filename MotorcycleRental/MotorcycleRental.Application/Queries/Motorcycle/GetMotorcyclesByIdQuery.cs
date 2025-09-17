using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries.Motorcycle
{
    public record GetMotorcyclesByIdQuery(string? Id) : IRequest<MotorcycleEntity>;

}
