using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Bookmarks;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.BookmarkActions
{
    public class CreateHandler : IRequestHandler<ApiActionAuthenticateRequest<BookmarkCreateInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public CreateHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<BookmarkCreateInputModel> request, CancellationToken cancellationToken)
        {
            var existed = await _dbContext.Bookmarks
                .AnyAsync(x => x.UserId == request.UserId.ToString() &&
                    x.DocumentId == request.Input.DocumentId &&
                    x.PageNumber == request.Input.PageNumer,
                    cancellationToken);

            if (existed)
            {
                return ApiResponse.CreateModel(HttpStatusCode.OK);
            }

            _dbContext.Bookmarks.Add(new Bookmarks
            {
                DocumentId = request.Input.DocumentId,
                UserId = request.UserId.ToString(),
                PageNumber = request.Input.PageNumer
            });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

