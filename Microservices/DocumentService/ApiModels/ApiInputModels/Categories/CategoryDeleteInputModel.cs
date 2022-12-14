using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Categories
{
    public class CategoryDeleteInputModel : IApiInput
    {
        [Required]
        public long CategoryId { get; set; }
    }
}

