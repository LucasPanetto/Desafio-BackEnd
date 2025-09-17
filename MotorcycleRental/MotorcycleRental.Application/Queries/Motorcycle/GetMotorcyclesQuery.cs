using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries.Motorcycle
{
    public record GetMotorcyclesQuery(string? Plate) : IRequest<IEnumerable<MotorcycleEntity>>;

}
