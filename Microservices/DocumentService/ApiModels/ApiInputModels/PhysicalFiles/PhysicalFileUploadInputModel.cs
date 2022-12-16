using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;
using Microsoft.AspNetCore.Http;

namespace DocumentService.ApiModels.ApiInputModels.PhysicalFiles
{
    public class PhysicalFileUploadInputModel : IApiInput
    {
        [Required]
        [MinLength(1)]
        public IFormFile[] Files { get; set; }
    }
}

