using MediatR;
using MotorcycleRental.Application.DTOs.Motorcycle;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.Motorcycle
{
    /// <summary>
    /// Comando para criar nova moto
    /// </summary>
    public record CreateMotorcycleCommand(
        [property: JsonPropertyName("identificador")] string Id,
        [property: JsonPropertyName("ano")] int Year,
        [property: JsonPropertyName("modelo")] string Model,
        [property: JsonPropertyName("placa")] string Plate
    ) : IRequest<MotorcycleDto>;
}
