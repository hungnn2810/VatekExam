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
    public class CreateHandler : IRequestHandler<ApiActionAuthenticateRequest<CategoryCreateInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public CreateHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<CategoryCreateInputModel> request, CancellationToken cancellationToken)
        {
            // Check duplicate name
            var duplicateName = await _dbContext.Categories
                .AnyAsync(x => x.CategoryName == request.Input.CategoryName, cancellationToken);

            if (duplicateName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DuplicatedCategoryName);
            }

            _dbContext.Categories.Add(new Categories
            {
                CategoryName = request.Input.CategoryName,
                Visible = request.Input.Visible,
                Deleted = false,
                CreatedBy = request.UserId.ToString(),
                CreatedAt = System.DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

