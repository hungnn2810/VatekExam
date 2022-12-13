using System.ComponentModel.DataAnnotations;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class AuthRegisterInputModel : IApiInput
    {
        [Required]
        [StringLength(128)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

        [Required]
        [StringLength(128)]
        public string UserName { get; set; }

        [Required]
        [StringLength(128)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

