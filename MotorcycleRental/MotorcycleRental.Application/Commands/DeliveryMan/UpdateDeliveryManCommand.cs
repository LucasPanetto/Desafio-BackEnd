using MediatR;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.DeliveryMan
{
    /// <summary>
    /// Comando para atualizar cnh de entregador
    /// </summary>
    public class UpdateDeliveryManCommand : IRequest<UpdateDeliveryManDto>
    {
        [JsonPropertyName("imagem_cnh")]
        [Required]
        public string CnhImage { get; init; }

        [JsonIgnore]
        public string? Id { get; set; }
    }
}
