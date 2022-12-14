using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Categories
{
    public class CategoryUpdateInputModel : IApiInput
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        public CategoryUpdateInput Details { get; set; }
    }

    public class CategoryUpdateInput
    {
        [Required]
        public string CategoryName { get; set; }
    }
}

