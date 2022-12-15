using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Bookmarks;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.BookmarkActions
{
    public class DeleteHandler : IRequestHandler<ApiActionAuthenticateRequest<BookmarkDeleteInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public DeleteHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<BookmarkDeleteInputModel> request, CancellationToken cancellationToken)
        {
            var bookmark = await _dbContext.Bookmarks
                .Where(x => x.UserId == request.UserId.ToString() &&
                    x.BookmarkId == request.Input.BookmarkId)
                .FirstOrDefaultAsync(cancellationToken);

            if (bookmark == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.BookmarkNotFound);
            }

            _dbContext.Bookmarks.Remove(bookmark);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

