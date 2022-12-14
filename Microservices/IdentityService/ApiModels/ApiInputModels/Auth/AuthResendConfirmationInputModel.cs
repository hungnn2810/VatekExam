using System;
using System.ComponentModel.DataAnnotations;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthResendConfirmationInputModel : IApiInput
    {
        [Required]
        public Guid ConfirmationCodeId { get; set; }
    }
}

