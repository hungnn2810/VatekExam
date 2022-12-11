using System;
namespace IdentityService.Commons.Constants
{
	public static class OAuthConstants
	{
        public static class ErrorMessages
        {
            public const string UsernameOrPasswordEmpty = "UsernameOrPasswordEmpty";
            public const string CannotFindUser = "CannotFindUser";
            public const string PasswordIncorrect = "PasswordIncorrect";
        }

        public static class ClaimTypes
        {
            public const string UserId = "userId";
        }
    }
}

