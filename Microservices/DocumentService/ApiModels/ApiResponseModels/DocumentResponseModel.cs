using System;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiResponseModels
{
    public class DocumentResponseModel : IApiResponseData
    {
        public long DocumentId { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
        public bool? Visible { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}

