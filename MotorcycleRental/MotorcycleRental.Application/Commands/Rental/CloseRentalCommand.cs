using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MotorcycleRental.Application.DTOs.Rental;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.Rental
{
    /// <summary>
    /// Comando para encerrar um aluguel
    /// </summary>
    /// 
    public class CloseRentalCommand : IRequest<CloseRentalDto>
    {
        [JsonPropertyName("data_devolucao")]
        [Required]
        public DateTime DevolutionDate { get; init; }

        [JsonIgnore]
        public int InternalId { get; set; }
    }
}
