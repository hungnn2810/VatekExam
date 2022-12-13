using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class DocumentDetailsResponseModel : IApiResponseData
    {
        public string CategoryName { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string FileUrl { get; set; }
    }
}

