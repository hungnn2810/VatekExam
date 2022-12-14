using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class SearchForOwnerHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentSearchForOwnerInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public SearchForOwnerHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentSearchForOwnerInputModel> request, CancellationToken cancellationToken)
        {
            var query = from d in _dbContext.Documents
                        where !d.Deleted &&
                        d.AuthorId == request.UserId.ToString() &&
                        (request.Input.CategoryId == null || !request.Input.CategoryId.HasValue ||
                            d.CategoryId == request.Input.CategoryId)
                        select new
                        {
                            d.DocumentId,
                            d.Title,
                            d.Category.CategoryName,
                            d.Visible,
                            d.CreatedAt,
                            d.UpdatedAt
                        };

            if (!string.IsNullOrEmpty(request.Input.Keyword) && request.Input.Keyword.Length >= 2)
            {
                query = from item in query
                        where item.Title.Contains(request.Input.Keyword)
                        select item;
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var requestPaging = new ApiResponsePaging(request.Input.PageSize, request.Input.PageNumber, totalItems);

            var result = await query
                .Skip(totalItems * (requestPaging.PageNumber - 1))
                .Take(requestPaging.PageNumber)
                .ToListAsync(cancellationToken);

            return ApiResponse.CreatePagingModel(
                result.Select(x => new DocumentResponseModel
                {
                    DocumentId = x.DocumentId,
                    Title = x.Title,
                    CategoryName = x.CategoryName,
                    Visible = x.Visible,
                    CreatedAtUtc = x.CreatedAt,
                    UpdatedAtUtc = x.UpdatedAt
                }).ToArray(),
                requestPaging);
        }
    }
}

