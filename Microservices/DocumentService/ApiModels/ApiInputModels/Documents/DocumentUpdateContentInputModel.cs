using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentUpdateContentInputModel : IApiInput
    {
        [Required]
        public long DocumentId { get; set; }

        public DocumentUpdateContentInput Details { get; set; }
    }

    public class DocumentUpdateContentInput
    {
        [Required]
        [MinLength(1)]
        public long[] PhysicalFileIds { get; set; }
    }
}

