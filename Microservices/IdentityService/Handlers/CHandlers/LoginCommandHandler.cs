using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Enums;
using Identity.EntityFramework;
using IdentityModel.Client;
using IdentityService.Commands;
using IdentityService.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers.CHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly TokenClient _tokenClient;

        public LoginCommandHandler(IdentityDbContext dbContext, TokenClient tokenClient)
        {
            _dbContext = dbContext;
            _tokenClient = tokenClient;
        }

        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _tokenClient.RequestPasswordTokenAsync(request.UserName, request.Password, "main.read_write", cancellationToken: cancellationToken);

            if (token.IsError)
            {
                if (token.ErrorDescription == OAuthConstants.ErrorMessages.CannotFindUser ||
                    token.ErrorDescription == OAuthConstants.ErrorMessages.PasswordIncorrect)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Messages = "Invalid user name or password"
                    };
                }

                return new AuthenticationResponse
                {
                    Success = false,
                    Messages = "Login failed"
                };
            }

            if (!token.Json.TryGetValue("userId", out var userIdValue))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Messages = "Login failed"
                };
            }

            if (string.IsNullOrEmpty(userIdValue.ToString()))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Messages = "Login failed"
                };
            }

            var user = await _dbContext.Users
                .Where(u => u.UserId == userIdValue.ToString())
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.UserStatusId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Messages = "Login failed"
                };
            }

            if (user.UserStatusId == (short)UserStatusEnum.WaitForVerify)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Messages = "User not verified"
                };
            }

            return new AuthenticationResponse
            {
                Success = true,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                UserId = user.UserId,
                UserName = user.UserName
            };
        }
    }
}

