namespace DocumentService.ApiModels.ApiResponseModels
{
    public class BookmarkResponseModel : DocumentResponseModel
    {
        public long BookmarkId { get; set; }
        public int PageNumber { get; set; }
    }
}

