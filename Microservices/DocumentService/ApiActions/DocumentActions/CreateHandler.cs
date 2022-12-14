using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
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
                .AnyAsync(x => !x.Deleted && x.CategoryId == request.Input.CategoryId, cancellationToken);

            if (!existCategory)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.CategoryNotFound);
            }

            var duplicateName = await _dbContext.Documents
                .AnyAsync(x => !x.Deleted &&
                    x.CategoryId == request.Input.CategoryId &&
                    x.Title == request.Input.Title, cancellationToken);

            if (duplicateName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DuplicatedDocumentTitle);
            }

            var fileIdCount = await _dbContext.PhysicalFiles
                .CountAsync(x => !x.Deleted && x.Active &&
                    request.Input.PhysicalFileIds.Contains(x.PhysicalFileId),
                    cancellationToken);

            if (fileIdCount != request.Input.PhysicalFileIds.Length)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.PhysicalFileNotFound);
            }
            #endregion

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var document = new Documents
            {
                CategoryId = request.Input.CategoryId,
                Title = request.Input.Title,
                AuthorId = request.UserId.ToString(),
                CreatedBy = request.UserId.ToString(),
                CreatedAt = System.DateTime.UtcNow
            };
            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var documentPages = request.Input.PhysicalFileIds
                .Select((fileId, index) => new DocumentPages
                {
                    DocumentId = document.DocumentId,
                    PhysicalFileId = fileId,
                    PageNumber = index + 1
                });
            await _dbContext.DocumentPages.BulkInsertAsync(documentPages);

            await transaction.CommitAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

