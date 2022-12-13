namespace DocumentService.Commons.Constants
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
            public const string UserType = "userType";
        }

        public static class ExtendGrantType
        {
            public const string ApiRequest = "Api_request";
        }
    }
}

