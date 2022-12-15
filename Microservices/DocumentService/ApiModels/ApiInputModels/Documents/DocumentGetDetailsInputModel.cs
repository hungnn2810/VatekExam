using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentGetDetailsInputModel : IApiInput
    {
        [Required]
        public long DocumentId { get; set; }
    }
}

