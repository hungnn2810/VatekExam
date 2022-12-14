using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Categories
{
    public class CategorySetVisibleInputModel : IApiInput
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        public CategorySetVisibleInput Details { get; set; }
    }

    public class CategorySetVisibleInput
    {
        [Required]
        public bool Visible { get; set; }
    }
}

