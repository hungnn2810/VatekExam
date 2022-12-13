using System;
using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.PhysicalFiles
{
    public class PhysicalFileRequestUploadInputModel : IApiInput
    {
        [Required]
        public string FileName { get; set; }

        // Max file size: 10MB
        [Required]
        [Range(1, 10485760)]
        public long FileLength { get; set; }
    }
}

