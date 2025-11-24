using Bib.Application.Features.Publisher.Command.CreatePublisher;
using Bib.Application.Features.Publisher.Command.DeletePublisher;
using Bib.Application.Features.Publisher.Command.UpdatePublisher;
using Bib.Application.Features.Publisher.Queries.Common;
using Bib.Application.Features.Publisher.Queries.GetPublisher;
using Bib.Application.Features.Publisher.Queries.GetPublisherById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bib.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly ISender _sender;

        public PublishersController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Busca todos os registros
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PublisherDto>), 200)]
        public async Task<IActionResult> GetPublishers()
        {
            var query = new GetPublishersQuery();
            var result = await _sender.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Busca um registro por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PublisherDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            var query = new GetPublisherByIdQuery { Id = id };
            var result = await _sender.Send(query);

            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Endpoint para criação de uma editora
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherCommand command)
        {
            var result = await _sender.Send(command);

            if (result.IsSuccess)
                return Created();

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Altera os dados do registro
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] UpdatePublisherCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _sender.Send(command);
            if (result.IsSuccess)
                return NoContent();

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Exclui um registro pelo seu Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var command = new DeletePublisherCommand { Id = id };
            await _sender.Send(command);
            return NoContent();
        }
    }
}
