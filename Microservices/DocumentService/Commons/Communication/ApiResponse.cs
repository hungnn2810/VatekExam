using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Commons.Communication
{
    public class JsonActionResult : JsonResult, IApiResponse
    {
        public JsonActionResult(object data, HttpStatusCode statusCode) : base(data)
        {
            StatusCode = (int)statusCode;
        }
    }

    public class ApiResponse
    {
        private ApiResponse()
        { }

        public static JsonActionResult CreateModel(HttpStatusCode statusCode = HttpStatusCode.OK, ApiResponseMetadata metadata = null)
        {
            return new JsonActionResult(new ApiResponseModel
            {
                Metadata = metadata ?? new ApiResponseMetadata
                {
                    Success = true
                }
            }, statusCode);
        }

        public static JsonActionResult CreateModel<TResponse>(TResponse data, HttpStatusCode statusCode = HttpStatusCode.OK, ApiResponseMetadata metadata = null)
            where TResponse : IApiResponseData, new()
        {
            return new JsonActionResult(new ApiResponseModel<TResponse>
            {
                Data = data,
                Metadata = metadata ?? new ApiResponseMetadata
                {
                    Success = true
                }
            }, statusCode);
        }

        public static JsonActionResult CreateErrorModel(HttpStatusCode statusCode, ApiErrorMessage message)
        {
            return new JsonActionResult(new
            {
                Metadata = new ApiResponseMetadata
                {
                    Success = false,
                    Message = message
                }
            }, statusCode);
        }
    }
}

