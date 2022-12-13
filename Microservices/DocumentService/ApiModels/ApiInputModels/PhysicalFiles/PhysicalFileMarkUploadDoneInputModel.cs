using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.PhysicalFiles
{
    public class PhysicalFileMarkUploadDoneInputModel : IApiInput
    {
        [Required]
        public long PhysicalFileId { get; set; }
    }
}

