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
    public class GetAllHandler : IRequestHandler<ApiActionAnonymousRequest<CategoryGetAllInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public GetAllHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<CategoryGetAllInputModel> request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Categories
                .Where(x => x.Visible)
                .Select(x => new CategoryResponseModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Visible = x.Visible
                })
                .ToArrayAsync(cancellationToken);

            return ApiResponse.CreateModel(data);
        }
    }
}

