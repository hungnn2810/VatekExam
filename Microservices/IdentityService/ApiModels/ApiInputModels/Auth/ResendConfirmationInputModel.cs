using System;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class ResendConfirmationInputModel : IApiInput
    {
        public Guid UserName { get; set; }
    }
}

