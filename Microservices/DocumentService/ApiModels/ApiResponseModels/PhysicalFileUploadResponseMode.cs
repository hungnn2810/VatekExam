using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class PhysicalFileUploadResponseModel : IApiResponseData
    {
        public long[] PhysicalFileIds { get; set; }
    }
}

