using MediatR;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using MotorcycleRental.Application.DTOs.Motorcycle;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.Motorcycle
{
    /// <summary>
    /// Comando para atualizar placa de moto
    /// </summary>
    public class UpdateMotorcycleCommand : IRequest
    {
        [JsonPropertyName("placa")]
        [Required]
        public string Plate { get; init; }

        [JsonIgnore]
        public string? Id { get; set; }
    }
}
