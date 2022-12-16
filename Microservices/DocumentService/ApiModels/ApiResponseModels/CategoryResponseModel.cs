using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class CategoryResponseModel : IApiResponseData
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Visible { get; set; }
    }
}

