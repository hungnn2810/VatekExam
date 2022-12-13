using System.ComponentModel.DataAnnotations;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthLoginInputModel : IApiInput
    {
        [Required]
        [StringLength(128)]
        public string UserName { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }
    }
}

