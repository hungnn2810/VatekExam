using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.Commons.Communication;
using DocumentService.Services;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class UpdateHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentUpdateInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly INotificationService _notificationService;

        public UpdateHandler(DocumentDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentUpdateInputModel> request, CancellationToken cancellationToken)
        {
            #region Validate input
            var document = await _dbContext.Documents
              .Where(x => !x.Deleted &&
                  x.AuthorId == request.UserId.ToString() &&
                  x.DocumentId == request.Input.DocumentId)
              .FirstOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DocumentNotFound);
            }

            var existCategory = await _dbContext.Categories
               .AnyAsync(x => !x.Deleted && x.CategoryId == request.Input.Details.CategoryId, cancellationToken);

            if (!existCategory)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.CategoryNotFound);
            }

            var duplicateName = await _dbContext.Documents
                .AnyAsync(x => !x.Deleted &&
                    x.CategoryId == request.Input.Details.CategoryId &&
                    x.DocumentId != request.Input.DocumentId &&
                    x.Title == request.Input.Details.Title, cancellationToken);

            if (duplicateName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DuplicatedDocumentTitle);
            }

            if (request.Input.Details.UpdatePhysicalFile && request.Input.Details.PhysicalFileIds != null &&
                request.Input.Details.PhysicalFileIds.Length > 0)
            {
                var fileIdCount = await _dbContext.PhysicalFiles
                .CountAsync(x => !x.Deleted && x.Active &&
                    request.Input.Details.PhysicalFileIds.Contains(x.PhysicalFileId),
                    cancellationToken);

                if (fileIdCount != request.Input.Details.PhysicalFileIds.Length)
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.PhysicalFileNotFound);
                }
            }
            #endregion

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            document.CategoryId = request.Input.Details.CategoryId;
            document.Title = request.Input.Details.Title;
            document.UpdatedBy = request.UserId.ToString();
            document.UpdatedAt = System.DateTime.UtcNow;
            _dbContext.Documents.Update(document);

            // Update mapping
            if (request.Input.Details.UpdatePhysicalFile && request.Input.Details.PhysicalFileIds != null &&
                request.Input.Details.PhysicalFileIds.Length > 0)
            {
                // Delete old mapping
                await _dbContext.DocumentPages
                    .Where(x => x.DocumentId == request.Input.DocumentId)
                    .DeleteFromQueryAsync(cancellationToken);

                // Insert new mapping
                var documentPages = request.Input.Details.PhysicalFileIds
                    .Select((fileId, index) => new DocumentPages
                    {
                        DocumentId = document.DocumentId,
                        PhysicalFileId = fileId,
                        PageNumber = index + 1
                    });
                await _dbContext.DocumentPages.BulkInsertAsync(documentPages);
            }
            await transaction.CommitAsync(cancellationToken);

            await _notificationService.TriggerUpdateDocument(request.Input.DocumentId, cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

