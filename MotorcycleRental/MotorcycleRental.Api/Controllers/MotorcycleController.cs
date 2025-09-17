using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRental.Application.Commands.Motorcycle;
using MotorcycleRental.Application.DTOs.Motorcycle;
using MotorcycleRental.Application.Queries.Motorcycle;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MotorcycleRental.Api.Controllers
{
    [ApiController]
    [Route("/motos")]
    public class MotorcycleController : ControllerBase
    {
        private readonly ILogger<MotorcycleController> _logger;
        private readonly IMediator _mediator;

        public MotorcycleController(ILogger<MotorcycleController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Cadastrar uma nova moto
        /// </summary>
        /// <param name="command">Dados para cadastrar uma moto</param>
        [HttpPost]
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMotorcycle([FromBody, Required] CreateMotorcycleCommand command)
        {
            _logger.LogInformation("Criando Moto: {@Command}", command);

            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            var motorcycle = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateMotorcycle), new { id = motorcycle.Id }, motorcycle);
        }

        /// <summary>
        /// Obter motos por id
        /// </summary>
        /// <param name="plate">Filtro por placa</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMotorcycleById([FromRoute] string id)
        {
            _logger.LogInformation("Obtendo Moto por id: {@Command}", id);

            if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(new { mensagem = "Request mal formada" });

                var result = await _mediator.Send(new GetMotorcyclesByIdQuery(id));

                if (result == null)
                    return NotFound(new { mensagem = "Moto não encontrada" });

                return Ok(result);
        }

        /// <summary>
        /// Obter motos (filtro opcional por placa)
        /// </summary>
        /// <param name="plate">Filtro por placa</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotorcycleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMotorcycles([FromQuery(Name = "placa")] string? plate)
        {
            _logger.LogInformation("Obtendo Moto: {@Command}", plate ?? "");

            var result = await _mediator.Send(new GetMotorcyclesQuery(plate));
                return Ok(result);
        }

        /// <summary>
        /// Atualizar placa de uma moto
        /// </summary>
        /// <param name="command">Dados para atualizar placa de uma moto</param>
        [HttpPut("{id}/placa")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMotorcycle([FromRoute] string id, [FromBody, Required] UpdateMotorcycleCommand command)
        {
            _logger.LogInformation("Atualizando Moto: {@Command}", command);

            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

                command.Id = id;
                await _mediator.Send(command);
                return Ok(new { mensagem = "Placa modificada com sucesso" });
        }

        /// <summary>
        /// Excluir uma moto
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMotorcycle([FromRoute] string id)
        {
            _logger.LogInformation("Excluindo Moto: {@Command}", id);

            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensagem = "Dados inválidos" });

                await _mediator.Send(new DeleteMotorcycleCommand(id));
                return Ok(new { mensagem = "Moto excluída com sucesso" });
        }

    }
}
