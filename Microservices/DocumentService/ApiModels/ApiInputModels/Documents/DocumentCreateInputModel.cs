using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Documents
{
    public class DocumentCreateInputModel : IApiInput
    {
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [Required]
        public long[] PhysicalFileIds { get; set; }
    }
}

