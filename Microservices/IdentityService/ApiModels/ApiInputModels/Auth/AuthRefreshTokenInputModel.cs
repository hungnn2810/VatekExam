using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthRefreshTokenInputModel : IApiInput
    {
        public string RefreshToken { get; set; }
    }
}

