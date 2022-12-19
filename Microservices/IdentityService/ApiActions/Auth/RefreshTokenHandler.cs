using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityService.ApiModels.ApiInputModels.Auth;
using IdentityService.ApiModels.ApiResponseModels;
using IdentityService.Commons.Communication;
using MediatR;

namespace IdentityService.ApiActions.Auth
{
    public class RefreshTokenHandler : IRequestHandler<ApiActionAnonymousRequest<AuthRefreshTokenInputModel>, IApiResponse>
    {
        private readonly TokenClient _tokenClient;

        public RefreshTokenHandler(TokenClient tokenClient)
        {
            _tokenClient = tokenClient;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<AuthRefreshTokenInputModel> request, CancellationToken cancellationToken)
        {
            var token = await _tokenClient.RequestRefreshTokenAsync(request.Input.RefreshToken, scope: "main.read_write offline_access", cancellationToken: cancellationToken);

            return ApiResponse.CreateModel(new UserTokenResponseModel
            {
                AccessToken = token.AccessToken,
                ExpiresIn = token.ExpiresIn,
                RefreshToken = token.RefreshToken,
                TokenType = token.TokenType
            });
        }
    }
}

