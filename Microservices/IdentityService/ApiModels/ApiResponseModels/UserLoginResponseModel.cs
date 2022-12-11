using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiResponseModels
{
    public class UserLoginResponseModel : IApiResponseData
    {
        public UserTokenResponseModel Token { get; set; }
        public UserResponseModel User { get; set; }
    }
}

