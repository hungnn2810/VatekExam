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
    public class SetVisibleHandler : IRequestHandler<ApiActionAuthenticateRequest<CategorySetVisibleInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public SetVisibleHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<CategorySetVisibleInputModel> request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Where(x => !x.Deleted && x.Visible != request.Input.Details.Visible)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.OK, ApiInternalErrorMessages.CategoryNotFound);
            }

            category.Visible = request.Input.Details.Visible;
            category.UpdatedBy = request.UserId.ToString();
            category.UpdatedAt = System.DateTime.UtcNow;
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

