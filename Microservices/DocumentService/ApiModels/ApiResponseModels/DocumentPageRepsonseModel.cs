using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class DocumentPageRepsonseModel : IApiResponseData
    {
        public long PhysicalFileId { get; set; }
        public string FileUrl { get; set; }
        public int PageNumber { get; set; }
    }
}

