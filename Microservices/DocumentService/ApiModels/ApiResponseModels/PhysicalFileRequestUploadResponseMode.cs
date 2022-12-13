using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class PhysicalFileRequestUploadResponseModel : IApiResponseData
    {
        public long PhysicalFileId { get; set; }
        public string PresignedUploadUrl { get; set; }
    }
}

