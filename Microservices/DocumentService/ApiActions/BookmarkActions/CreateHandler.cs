using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Bookmarks;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;

namespace DocumentService.ApiActions.BookmarkActions
{
    public class CreateHandler : IRequestHandler<ApiActionAuthenticateRequest<BookmarkCreateInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public CreateHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IApiResponse> Handle(ApiActionAuthenticateRequest<BookmarkCreateInputModel> request, CancellationToken cancellationToken)
        {



            throw new NotImplementedException();
        }
    }
}

