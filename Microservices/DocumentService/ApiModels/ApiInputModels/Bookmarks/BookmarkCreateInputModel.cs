using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Bookmarks
{
    public class BookmarkCreateInputModel : IApiInput
    {
        [Required]
        public long DocumentId { get; set; }

        [Required]
        public int PageNumer { get; set; }
    }
}

