using System.ComponentModel.DataAnnotations;
using DocumentService.Commons.Communication;

namespace DocumentService.ApiModels.ApiInputModels.Bookmarks
{
    public class BookmarkDeleteInputModel : IApiInput
    {
        [Required]
        public long BookmarkId { get; set; }
    }
}

