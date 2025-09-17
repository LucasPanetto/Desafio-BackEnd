using MediatR;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.DeliveryMan
{
    /// <summary>
    /// Comando para criar novo entregador
    /// </summary>
    public class CreateDeliveryManCommand : IRequest<CreateDeliveryManDto>
    {
        [JsonPropertyName("identificador")]
        [Required]
        public string Id { get; init; }

        [JsonPropertyName("nome")]
        [Required]
        public string Name { get; init; }

        [JsonPropertyName("cnpj")]
        [Required]
        public string Cnpj { get; init; }

        [JsonPropertyName("data_nascimento")]
        [Required]
        public DateTime Birthday { get; init; }

        [JsonPropertyName("numero_cnh")]
        [Required]
        public string CnhNumber { get; init; }

        [Required]
        [JsonPropertyName("tipo_cnh")]
        [RegularExpression("(?i)^(a|b|ab)$", ErrorMessage = "CNH deve ser A, B ou AB")]
        public string CnhType { get; init; }

        [JsonPropertyName("imagem_cnh")]
        [Required]
        public string CnhImage { get; init; }
    }
}
