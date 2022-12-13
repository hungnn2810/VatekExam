using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Commons.Communication
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
        public string Message { get; set; }

        public ApiResponseMetadata(bool success = true, string message = null)
        {
            Success = success;
            Message = message;
        }
    }

    public class ApiResponsePaging
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalItem { get; set; }

        public ApiResponsePaging(int pageSize, int pageNumber, int totalItems)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalItem = totalItems;
        }
    }

    public class ApiResponseArrayWithPaging<T>
        where T : IApiResponseData
    {
        public ApiResponsePaging Paging { get; set; }
        public IEnumerable<T> PageData { get; set; }

        public ApiResponseArrayWithPaging(IEnumerable<T> pageData, ApiResponsePaging paging)
        {
            PageData = pageData;
            Paging = paging;
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

    public class ApiArrayResponseModel<TResponse>
        where TResponse : IApiResponseData, new()
    {
        public ApiResponseMetadata Metadata { get; set; }
        public IEnumerable<TResponse> Data { get; set; }
    }

    public class ApiPagingResponseModel<TResponse>
        where TResponse : IApiResponseData, new()
    {
        public ApiResponseMetadata Metadata { get; set; }
        public ApiResponseArrayWithPaging<TResponse> Data { get; set; }
    }
}

