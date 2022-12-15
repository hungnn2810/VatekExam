using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentGetPageContentInputModel : IApiInput
    {
        [Required]
        public long DocumentId { get; set; }

        [Required]
        public long PageNumber { get; set; }
    }
}

