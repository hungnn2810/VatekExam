using System;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiResponseModels
{
    public class UserResponseModel : IApiResponseData
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}

