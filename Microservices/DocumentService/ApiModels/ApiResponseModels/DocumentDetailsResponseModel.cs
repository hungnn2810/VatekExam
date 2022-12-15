using System;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class DocumentDetailsResponseModel : IApiResponseData
    {
        public long DocumentId { get; set; }
        public string Title { get; set; }
        public long CategoryId { get; set; }
        public bool? Visible { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public DocumentPageRepsonseModel[] Pages { get; set; }
    }
}

