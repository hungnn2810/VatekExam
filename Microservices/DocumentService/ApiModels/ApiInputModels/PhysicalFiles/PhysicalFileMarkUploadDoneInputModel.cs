using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.PhysicalFiles
{
    public class PhysicalFileMarkUploadDoneInputModel : IApiInput
    {
        [Required]
        public long[] PhysicalFileIds { get; set; }
    }
}

