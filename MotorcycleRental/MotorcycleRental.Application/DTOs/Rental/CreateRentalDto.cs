using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.DTOs.Rental
{
    public record CreateRentalDto(
        [property: JsonPropertyName("identificador_interno")] int InternalId,
        [property: JsonPropertyName("entregador_id")] string DeliveryManId,
        [property: JsonPropertyName("moto_id")] string MotorcycleId,
        [property: JsonPropertyName("data_inicio")] DateTime StartDate,
        [property: JsonPropertyName("data_termino")] DateTime EndDate,
        [property: JsonPropertyName("data_previsao_termino")] DateTime ExpectedEndDate,
        [property: JsonPropertyName("plano")] int Plan
    );

}
