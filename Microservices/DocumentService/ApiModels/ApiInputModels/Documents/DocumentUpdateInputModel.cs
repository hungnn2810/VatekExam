using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentUpdateInputModel : IApiInput
    {
        [Required]
        public long DocumentId { get; set; }

        [Required]
        public DocumentUpdateInput Details { get; set; }
    }

    public class DocumentUpdateInput
    {
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [Required]
        public bool UpdatePhysicalFile { get; set; }

        public long[] PhysicalFileIds { get; set; }
    }
}

