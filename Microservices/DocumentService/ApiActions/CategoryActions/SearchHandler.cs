using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Categories;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.CategoryActions
{
    public class SearchHandler : IRequestHandler<ApiActionAuthenticateRequest<CategorySearchInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public SearchHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<CategorySearchInputModel> request, CancellationToken cancellationToken)
        {
            var query = from c in _dbContext.Categories
                        select c;

            if (!string.IsNullOrEmpty(request.Input.Keyword) && request.Input.Keyword.Length >= 2)
            {
                query = from item in query
                        where item.CategoryName.Contains(request.Input.Keyword)
                        select item;
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var requestPaging = new ApiResponsePaging(request.Input.PageSize, request.Input.PageNumber, totalItems);

            var result = await query
                .Skip(totalItems * (requestPaging.PageNumber - 1))
                .Take(requestPaging.PageSize)
                .ToListAsync(cancellationToken);

            return ApiResponse.CreatePagingModel(
                result.Select(x => new CategoryResponseModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Visible = x.Visible
                }).ToArray(),
                requestPaging);
        }
    }
}

