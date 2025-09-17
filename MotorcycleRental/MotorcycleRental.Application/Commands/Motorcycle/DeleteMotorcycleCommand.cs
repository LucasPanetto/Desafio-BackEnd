using MediatR;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using MotorcycleRental.Application.DTOs.Motorcycle;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotorcycleRental.Application.Commands.Motorcycle
{
    /// <summary>
    /// Comando para excluir uma moto
    /// </summary>
    public record DeleteMotorcycleCommand(string Id) : IRequest;

}
