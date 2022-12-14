using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentSearchForOwnerInputModel : IApiInput
    {
        [Range(1, 100)]
        public int PageSize { get; set; }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        public string Keyword { get; set; }

        public long? CategoryId { get; set; }
    }
}

