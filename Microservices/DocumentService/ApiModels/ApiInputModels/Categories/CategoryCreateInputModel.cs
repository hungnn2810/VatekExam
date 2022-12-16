using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Categories
{
    public class CategoryCreateInputModel : IApiInput
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public bool Visible { get; set; }
    }
}

