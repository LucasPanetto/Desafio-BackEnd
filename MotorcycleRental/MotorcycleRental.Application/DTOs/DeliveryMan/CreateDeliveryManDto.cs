using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.DTOs.DeliveryMan
{
    public record CreateDeliveryManDto(
           [property: JsonPropertyName("identificador_interno")] int InternalId,
           [property: JsonPropertyName("identificador")] string Id,
           [property: JsonPropertyName("nome")] string Name,
           [property: JsonPropertyName("cnpj")] string Cnpj,
           [property: JsonPropertyName("data_nascimento")] DateTime BirthDate,
           [property: JsonPropertyName("numero_cnh")] string CnhNumber,
           [property: JsonPropertyName("tipo_cnh")] string CnhType,
           [property: JsonPropertyName("imagem_cnh")] string CnhImage
    );
}
