using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commons.Communication
{
    public interface IApiInput
    { }

    public interface IApiResponse : IActionResult
    { }

    public interface IApiResponseData
    { }

    public class ApiResponseMetadata
    {
        public bool Success { get; set; }
        public ApiErrorMessage Message { get; set; }

        public ApiResponseMetadata(bool success = true, ApiErrorMessage message = null)
        {
            Success = success;
            Message = message;
        }
    }

    public class ApiResponseModel
    {
        public ApiResponseMetadata Metadata { get; set; }
    }

    public class ApiResponseModel<TResponse>
        where TResponse : IApiResponseData, new()
    {
        public ApiResponseMetadata Metadata { get; set; }
        public TResponse Data { get; set; }
    }
}

