using System;
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
    public class UpdateHandler : IRequestHandler<ApiActionAuthenticateRequest<CategoryUpdateInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public UpdateHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<CategoryUpdateInputModel> request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Where(x => x.CategoryId == request.Input.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.CategoryNotFound);
            }

            // Check duplicate name
            var duplicateName = await _dbContext.Categories
                .AnyAsync(x => !x.Deleted &&
                    x.CategoryName == request.Input.Details.CategoryName &&
                    x.CategoryId != request.Input.CategoryId, cancellationToken);

            if (duplicateName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DuplicatedCategoryName);
            }

            category.CategoryName = request.Input.Details.CategoryName;
            category.UpdatedBy = request.UserId.ToString();
            category.UpdatedAt = DateTime.UtcNow;

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

