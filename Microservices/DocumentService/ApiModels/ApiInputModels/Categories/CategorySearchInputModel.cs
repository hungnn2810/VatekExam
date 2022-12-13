using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Categories
{
    public class CategorySearchInputModel : IApiInput
    {
        [Required]
        public int PageSize { get; set; }

        [Required]
        public int PageNumber { get; set; }

        public string Keyword { get; set; }
    }
}

