using System;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthResendConfirmationInputModel : IApiInput
    {
        public Guid UserName { get; set; }
    }
}

