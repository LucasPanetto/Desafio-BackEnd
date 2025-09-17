using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands.DeliveryMan;
using MotorcycleRental.Application.Commands.Rental;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using MotorcycleRental.Application.DTOs.Motorcycle;
using MotorcycleRental.Application.DTOs.Rental;
using MotorcycleRental.Application.Handlers.DeliveryMan;
using MotorcycleRental.Application.Queries.Motorcycle;
using MotorcycleRental.Application.Queries.Rental;
using MotorcycleRental.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MotorcycleRental.Api.Controllers
{
    [ApiController]
    [Route("/locacao")]
    public class RentalController : Controller
    {
        private readonly ILogger<RentalController> _logger;
        private readonly IMediator _mediator;

        public RentalController(ILogger<RentalController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Cadastrar um novo aluguel
        /// </summary>
        /// <param name="command">Dados para cadastrar novo alugel</param>
        [ProducesResponseType(typeof(CreateRentalDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody, Required] CreateRentalCommand command)
        {
            _logger.LogInformation("Criando Aluguel: {@Command}", command);

            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            var rentalEntity = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateRental), new { id = rentalEntity.InternalId }, rentalEntity);
        }

        /// <summary>
        /// Obter aluguel
        /// </summary>
        /// <param name="id">Filtro por id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetRentalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRentalById([FromRoute] string id)
        {
            _logger.LogInformation("Obtendo Aluguel: {@Command}", id);

            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensagem = "Dados inválidos" });

            if (!int.TryParse(id, out var rentalId))
                return BadRequest(new { mensagem = "Id inválido, deve ser numérico" });

            var result = await _mediator.Send(new GetRentalByIdQuery(rentalId));

            if (result == null)
                return NotFound(new { mensagem = "Aluguel não encontrado" });

            return Ok(result);
        }

        /// <summary>
        /// Finalizar aluguel
        /// </summary>
        /// <param name="id">Filtro por id</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CloseRentalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CloseRentalById([FromRoute] string id, [FromBody, Required] CloseRentalCommand command)
        {
            _logger.LogInformation("Encerrando Aluguel: {@Command}", command);

            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensagem = "Dados inválidos" });

            if (!int.TryParse(id, out var rentalId))
                return BadRequest(new { mensagem = "Id inválido, deve ser numérico" });

            command.InternalId = rentalId;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new { mensagem = "Aluguel não encontrado" });

            return Ok(result);
        }

    }
}
