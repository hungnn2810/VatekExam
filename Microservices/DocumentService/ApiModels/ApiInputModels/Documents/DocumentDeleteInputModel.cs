using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentDeleteInputModel : IApiInput
    {
        [Required]
        public long DocumentId { get; set; }
    }
}

