using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.DTOs.Motorcycle
{
    public record MotorcycleDto(
        [property: JsonPropertyName("identificador")] string Id,
        [property: JsonPropertyName("ano")] int Year,
        [property: JsonPropertyName("modelo")] string Model,
        [property: JsonPropertyName("placa")] string Plate
    );
}
