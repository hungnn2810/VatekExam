using System;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiResponseModels
{
    public class ConfirmationResponseModel : IApiResponseData
    {
        public Guid ConfirmationCodeId { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}

