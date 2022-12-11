using IdentityService.Commons.Communication;

namespace IdentityService.ApiModel.ApiErrorMessages
{
    public static class ApiInternalErrorMessages
    {
        public static ApiErrorMessage LoginFailed = new ApiErrorMessage
        {
            Code = "ERR_1001",
            Value = "Login failed"
        };

        public static ApiErrorMessage InvalidUsernameOrPassword = new ApiErrorMessage
        {
            Code = "ERR_1002",
            Value = "Invalid user name or password"
        };

        public static ApiErrorMessage UserNotVerified = new ApiErrorMessage
        {
            Code = "ERR_1003",
            Value = "User not verified"
        };

        public static ApiErrorMessage EmailAlreadyExisted = new ApiErrorMessage
        {
            Code = "ERR_1004",
            Value = "Email already existed"
        };

        public static ApiErrorMessage UserNameAlreadyExisted = new ApiErrorMessage
        {
            Code = "ERR_1005",
            Value = "User name already existed"
        };

        public static ApiErrorMessage InvalidConfirmationCode = new ApiErrorMessage
        {
            Code = "ERR_1006",
            Value = "Invalid confirmation code"
        };

        public static ApiErrorMessage ConfirmationCodeExpired = new ApiErrorMessage
        {
            Code = "ERR_1007",
            Value = "Confirmation code expired"
        };

        public static ApiErrorMessage UserAlreadyVerified = new ApiErrorMessage
        {
            Code = "ERR_1008",
            Value = "User already verified"
        };

        #region Not found errors
        public static ApiErrorMessage UserNotFound = new ApiErrorMessage
        {
            Code = "ERR_4001",
            Value = "User not found"
        };
        #endregion
    }
}

