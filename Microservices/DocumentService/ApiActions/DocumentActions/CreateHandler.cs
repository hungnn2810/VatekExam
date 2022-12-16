using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class CreateHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentCreateInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public CreateHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentCreateInputModel> request, CancellationToken cancellationToken)
        {
            #region Validate input
            var existCategory = await _dbContext.Categories
                .AnyAsync(x => x.CategoryId == request.Input.CategoryId, cancellationToken);

            if (!existCategory)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.CategoryNotFound);
            }

            var duplicateName = await _dbContext.Documents
                .AnyAsync(x => x.CategoryId == request.Input.CategoryId &&
                    x.Title == request.Input.Title, cancellationToken);

            if (duplicateName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DuplicatedDocumentTitle);
            }
            #endregion
            var document = new Documents
            {
                CategoryId = request.Input.CategoryId,
                Title = request.Input.Title,
                AuthorId = request.UserId.ToString(),
                Visible = true,
                Deleted = false,
                CreatedBy = request.UserId.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(new DocumentResponseModel
            {
                DocumentId = document.DocumentId,
                CreatedAtUtc = document.CreatedAt
            });
        }
    }
}

