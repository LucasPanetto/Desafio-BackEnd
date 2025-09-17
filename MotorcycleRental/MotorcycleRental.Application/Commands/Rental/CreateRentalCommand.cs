using MediatR;
using MotorcycleRental.Application.DTOs.Rental;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.Rental
{
    /// <summary>
    /// Comando para criar um aluguel
    /// </summary>
    public record CreateRentalCommand(
        [property: JsonPropertyName("entregador_id")] string DeliveryManId,
        [property: JsonPropertyName("moto_id")] string MotorcycleId,
        [property: JsonPropertyName("data_inicio")] DateTime StartDate,
        [property: JsonPropertyName("data_termino")] DateTime EndDate,
        [property: JsonPropertyName("data_previsao_termino")] DateTime ExpectedEndDate,
        [property: JsonPropertyName("plano")] int Plan
    ) : IRequest<CreateRentalDto>;
}
