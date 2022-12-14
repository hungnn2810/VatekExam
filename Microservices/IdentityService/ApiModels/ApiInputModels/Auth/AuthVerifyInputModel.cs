using System;
using System.ComponentModel.DataAnnotations;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthVerifyInputModel : IApiInput
    {
        [Required]
        public Guid ConfirmationCodeId { get; set; }

        [Required]
        public string ConfirmationCode { get; set; }
    }
}

