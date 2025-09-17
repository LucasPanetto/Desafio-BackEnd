using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.DTOs.Rental
{
    public record GetRentalDto(
        [property: JsonPropertyName("identificador")] int InternalId,
        [property: JsonPropertyName("valor_diaria")] double ValueDay,
        [property: JsonPropertyName("valor_total")] double? ValueTotal,
        [property: JsonPropertyName("entregador_id")] string DeliveryManId,
        [property: JsonPropertyName("moto_id")] string MotorcycleId,
        [property: JsonPropertyName("data_inicio")] DateTime StartDate,
        [property: JsonPropertyName("data_termino")] DateTime EndDate,
        [property: JsonPropertyName("data_previsao_termino")] DateTime ExpectedEndDate,
        [property: JsonPropertyName("data_devolucao")] DateTime? DevolutionDate
    )
    {
    }
}
