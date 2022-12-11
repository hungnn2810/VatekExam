using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class LoginInputModel : IApiInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

