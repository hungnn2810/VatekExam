using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiResponseModels
{
    public class UserTokenResponseModel : IApiResponseData
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
    }
}

