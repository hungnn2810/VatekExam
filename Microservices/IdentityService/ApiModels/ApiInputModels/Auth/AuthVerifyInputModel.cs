using System;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthVerifyInputModel : IApiInput
    {
        public Guid ConfirmationCodeId { get; set; }
        public string ConfirmationCode { get; set; }
    }
}

