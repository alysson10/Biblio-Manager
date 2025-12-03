using Bib.Application.Common.Models;
using Bib.Application.Features.User.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeuProjeto.Security;
using System.Security.Cryptography;

namespace Bib.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;

        public UserController(ISender sender, IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }

        /// <summary>
        /// Generate password hash
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("encrypt")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public Task<IActionResult> Encrypt([FromBody] DataCommand command)
        {
            try
            {
                var secretKey = _configuration["SecretKey"] ?? string.Empty;
                var key = Convert.FromBase64String(secretKey);

                using var encryptionService = new AesGcmEncryptionService(key);                
                var encrypted = encryptionService.EncryptToBase64(command.Text);

                var passwordHash = string.Concat(encrypted.Ciphertext, ":", encrypted.Nonce, ":", encrypted.Tag);

                var result = Result<string>.Success(passwordHash);
                return Task.FromResult<IActionResult>(Ok(result));
            }
            catch (CryptographicException ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex.Message));
            }            
        }

        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _sender.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}