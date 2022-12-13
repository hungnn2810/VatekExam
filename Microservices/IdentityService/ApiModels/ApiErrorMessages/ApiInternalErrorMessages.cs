using IdentityService.Commons.Communication;

namespace IdentityService.ApiModel.ApiErrorMessages
{
    public static class ApiInternalErrorMessages
    {
        public static ApiErrorMessage LoginFailed = new ApiErrorMessage
        {
            Value = "Login failed"
        };

        public static ApiErrorMessage InvalidUsernameOrPassword = new ApiErrorMessage
        {
            Value = "Invalid user name or password"
        };

        public static ApiErrorMessage UserNotVerified = new ApiErrorMessage
        {
            Value = "User not verified"
        };

        public static ApiErrorMessage EmailAlreadyExisted = new ApiErrorMessage
        {
            Value = "Email already existed"
        };

        public static ApiErrorMessage UserNameAlreadyExisted = new ApiErrorMessage
        {
            Value = "User name already existed"
        };

        public static ApiErrorMessage InvalidConfirmationCode = new ApiErrorMessage
        {
            Value = "Invalid confirmation code"
        };

        public static ApiErrorMessage ConfirmationCodeExpired = new ApiErrorMessage
        {
            Value = "Confirmation code expired"
        };

        public static ApiErrorMessage UserAlreadyVerified = new ApiErrorMessage
        {
            Value = "User already verified"
        };

        #region Not found errors
        public static ApiErrorMessage UserNotFound = new ApiErrorMessage
        {
            Value = "User not found"
        };
        #endregion
    }
}

