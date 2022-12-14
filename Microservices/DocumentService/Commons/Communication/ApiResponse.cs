using System.Collections.Generic;
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

        public static JsonActionResult CreateModel<TResponse>(IEnumerable<TResponse> data, HttpStatusCode statusCode = HttpStatusCode.OK, ApiResponseMetadata metadata = null)
             where TResponse : IApiResponseData, new()
        {
            return new JsonActionResult(new ApiArrayResponseModel<TResponse>
            {
                Data = data,
                Metadata = metadata ?? new ApiResponseMetadata
                {
                    Success = true
                }
            }, statusCode);
        }

        public static JsonActionResult CreatePagingModel<TResponse>(IEnumerable<TResponse> data, ApiResponsePaging paging, HttpStatusCode statusCode = HttpStatusCode.OK, ApiResponseMetadata metadata = null)
            where TResponse : IApiResponseData, new()
        {
            return new JsonActionResult(new ApiPagingResponseModel<TResponse>
            {
                Data = new ApiResponseArrayWithPaging<TResponse>(data, paging),
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
                    Message = message.Value
                }
            }, statusCode);
        }
    }
}

