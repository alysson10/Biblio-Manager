using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SeuProjeto.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.User.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var decryptedPassword = await DecriptPassword(command.Password);
            if (string.IsNullOrEmpty(decryptedPassword))
                return Result<LoginResponse>.Failure(Error.Unauthorized);
            
            var condition = $"Login = '{command.Login}' ";
            var user = await _unitOfWork.UserRepository.GetByConditionAsync(condition);

            if (user == null || !VerifyPassword(decryptedPassword, user.Password))
                return Result<LoginResponse>.Failure(Error.Unauthorized);

            var expiration = DateTime.Now.AddHours(2);
            var token = GenerateToken(user, expiration);
            if (string.IsNullOrEmpty(token))
                return Result<LoginResponse>.Failure(Error.Unauthorized);

            var response = new LoginResponse(token, expiration, user.Login);            

            return Result<LoginResponse>.Success(response);
        }

        private string GenerateToken(Domain.Entities.User user, DateTime expiration)
        {
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
                return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                NotBefore = DateTime.Now
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private async Task<string> DecriptPassword(string password)
        {
            var secretKey = _configuration["SecretKey"] ?? string.Empty;
            var key = Convert.FromBase64String(secretKey);

            using var encryptionService = new AesGcmEncryptionService(key);

            var parts = password.Split(':');
            if (parts.Length != 3)
                return string.Empty;

            var encrypted = new StringEncryptedResult
            {
                Ciphertext = parts[0],
                Nonce = parts[1],
                Tag = parts[2]
            };

            return await Task.FromResult(encryptionService.DecryptFromBase64(encrypted));
        }
    }
}
