using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Identity;
using IdentityModel.Client;
using IdentityService.ApiModel.ApiErrorMessages;
using IdentityService.ApiModels.ApiInputModels.Auth;
using IdentityService.ApiModels.ApiResponseModels;
using IdentityService.Commoms.Enums;
using IdentityService.Commons.Communication;
using IdentityService.Commons.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers.CHandlers
{
    public class LoginHandler : IRequestHandler<ApiActionAnonymousRequest<AuthLoginInputModel>, IApiResponse>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly TokenClient _tokenClient;

        public LoginHandler(IdentityDbContext dbContext, TokenClient tokenClient)
        {
            _dbContext = dbContext;
            _tokenClient = tokenClient;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<AuthLoginInputModel> request, CancellationToken cancellationToken)
        {
            var token = await _tokenClient.RequestPasswordTokenAsync(request.Input.UserName, request.Input.Password, scope: "main.read_write offline_access", cancellationToken: cancellationToken);

            if (token.IsError)
            {
                if (token.ErrorDescription == OAuthConstants.ErrorMessages.CannotFindUser ||
                    token.ErrorDescription == OAuthConstants.ErrorMessages.PasswordIncorrect)
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.InvalidUsernameOrPassword);
                }

                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.LoginFailed);
            }

            if (!token.Json.TryGetValue("userId", out var userIdValue))
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.LoginFailed);
            }

            if (string.IsNullOrEmpty(userIdValue.ToString()))
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.LoginFailed);
            }

            var user = await _dbContext.Users
                .Where(u => u.UserId == userIdValue.ToString())
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.UserStatusId,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.LoginFailed);
            }

            if (user.UserStatusId == (short)UserStatusEnum.WaitForVerify)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.UserNotVerified);
            }

            return ApiResponse.CreateModel(new UserLoginResponseModel
            {
                Token = new UserTokenResponseModel
                {
                    AccessToken = token.AccessToken,
                    ExpiresIn = token.ExpiresIn,
                    RefreshToken = token.RefreshToken,
                    TokenType = token.TokenType
                },
                User = new UserResponseModel
                {
                    UserId = Guid.Parse(user.UserId),
                    UserName = user.UserName
                }
            });
        }
    }
}

