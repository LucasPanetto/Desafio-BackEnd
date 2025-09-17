using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.DTOs.Rental
{
    public record CloseRentalDto(
        [property: JsonPropertyName("mensagem")] string Message
    );

}
