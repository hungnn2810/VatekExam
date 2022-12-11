using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.EntityFramework;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdentityService.Commons.Constants;
using IdentityService.Commons.Utils;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IdentityDbContext _dbContext;

        public ResourceOwnerPasswordValidator(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, OAuthConstants.ErrorMessages.UsernameOrPasswordEmpty);
                return;
            }

            var user = await (from u in _dbContext.Users
                              where !u.Deleted &&
                              u.UserName == context.UserName
                              select u).FirstOrDefaultAsync();
            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, OAuthConstants.ErrorMessages.CannotFindUser);
                return;
            }

            if (StringHelper.HashPassword(context.Password) != user.PasswordHashed)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, OAuthConstants.ErrorMessages.PasswordIncorrect);
                return;
            }

            var customClaims = new List<Claim>()
            {
                new Claim(OAuthConstants.ClaimTypes.UserId, user.UserId.ToString()),
            };

            context.Result = new GrantValidationResult(
                subject: user.UserName,
                authenticationMethod: "password",
                claims: customClaims,
                identityProvider: "local",
                customResponse: new Dictionary<string, object>() { { "userId", user.UserId } }
            );
        }
    }
}

