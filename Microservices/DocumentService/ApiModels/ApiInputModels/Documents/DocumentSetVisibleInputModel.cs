using System.ComponentModel.DataAnnotations;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentSetVisibleInputModel
    {
        [Required]
        public long DocumentId { get; set; }

        [Required]
        public DocumentSetVisibleInput Details { get; set; }
    }

    public class DocumentSetVisibleInput
    {
        [Required]
        public bool Visible { get; set; }
    }
}

