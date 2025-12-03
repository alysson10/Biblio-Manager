using Bib.Application.Common.Models;
using MediatR;
using System;

namespace Bib.Application.Features.User.Command
{
    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public record LoginResponse(string Token, DateTime Expiration, string Username);
    
}
