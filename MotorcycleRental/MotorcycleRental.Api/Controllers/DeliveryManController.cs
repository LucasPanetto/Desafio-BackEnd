using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands.DeliveryMan;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Api.Controllers
{
    [ApiController]
    [Route("/entregadores")]
    public class DeliveryManController : ControllerBase
    {
        private readonly ILogger<DeliveryManController> _logger;
        private readonly IMediator _mediator;

        public DeliveryManController(ILogger<DeliveryManController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Cadastrar um novo entregador
        /// </summary>
        /// <param name="command">Dados para cadastrar entregador</param>
        [ProducesResponseType(typeof(CreateDeliveryManDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateDeliveryMan([FromBody, Required] CreateDeliveryManCommand command)
        {
            _logger.LogInformation("Criando entregador: {@Command}", command);

            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            var deliveryMan = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateDeliveryMan), new { id = deliveryMan.Id }, deliveryMan);
        }

        /// <summary>
        /// Atualiza a CNH de um entregador
        /// </summary>
        /// <param name="id">Identificador do entregador</param>
        /// <param name="command">Dados para atualização da CNH</param>
        [ProducesResponseType(typeof(UpdateDeliveryManDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> UpdateCnhDeliveryMan(
            [FromRoute] string id,
            [FromBody, Required] UpdateDeliveryManCommand command)
        {
            _logger.LogInformation("Atualiando CNH entregador: {@Command}", command);

            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            command.Id = id;

            var deliveryMan = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateDeliveryMan), new { id = deliveryMan.Id }, deliveryMan);
        }


    }
}
