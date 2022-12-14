using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Categories;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.CategoryActions
{
    public class DeleteHandler : IRequestHandler<ApiActionAuthenticateRequest<CategoryDeleteInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public DeleteHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<CategoryDeleteInputModel> request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
               .Where(x => !x.Deleted)
               .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.OK, ApiInternalErrorMessages.CategoryNotFound);
            }

            category.Deleted = true;
            category.UpdatedBy = request.UserId.ToString();
            category.UpdatedAt = System.DateTime.UtcNow;
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

