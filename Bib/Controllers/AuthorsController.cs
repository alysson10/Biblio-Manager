using Bib.Application.Features.Author.Command.CreateAuthor;
using Bib.Application.Features.Author.Command.DeleteAuthor;
using Bib.Application.Features.Author.Command.UpdateAuthor;
using Bib.Application.Features.Author.Queries.GetAuthorById;
using Bib.Application.Features.Author.Queries.GetAuthors;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bib.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthorsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuthorDto>), 200)]
        public async Task<IActionResult> GetAuthors()
        {
            var query = new GetAuthorsQuery();
            var result = await _sender.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Get author by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AuthorDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var query = new GetAuthorByIdQuery { Id = id };
            var result = await _sender.Send(query);

            return result.Value is not null ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> CreatePublisher([FromBody] CreateAuthorCommand command)
        {
            var result = await _sender.Send(command);

            if (result.IsSuccess)
                return Created();

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Update an author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] UpdateAuthorCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _sender.Send(command);
            if (result.IsSuccess)
                return NoContent();

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Delete an author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var command = new DeleteAuthorCommand { Id = id };
            await _sender.Send(command);
            return NoContent();
        }
    }
}
