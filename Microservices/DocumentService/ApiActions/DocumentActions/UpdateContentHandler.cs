﻿using System;
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
    public class UpdateContentHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentUpdateContentInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly INotificationService _notificationService;

        public UpdateContentHandler(DocumentDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentUpdateContentInputModel> request, CancellationToken cancellationToken)
        {
            #region Validate input
            var document = await _dbContext.Documents
              .Where(x => x.AuthorId == request.UserId.ToString() &&
                  x.DocumentId == request.Input.DocumentId)
              .FirstOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DocumentNotFound);
            }

            if (request.Input.Details.PhysicalFileIds != null && request.Input.Details.PhysicalFileIds.Length > 0)
            {
                var fileIdCount = await _dbContext.PhysicalFiles
                    .CountAsync(x => x.Active && request.Input.Details.PhysicalFileIds.Contains(x.PhysicalFileId),
                        cancellationToken);

                if (fileIdCount != request.Input.Details.PhysicalFileIds.Length)
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.PhysicalFileNotFound);
                }
            }
            #endregion

            var fileIds = await _dbContext.PhysicalFiles
                .Where(x => x.DocumentId == request.Input.DocumentId)
                .Select(x => x.PhysicalFileId)
                .ToListAsync(cancellationToken);

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            if (request.Input.Details.PhysicalFileIds != null && request.Input.Details.PhysicalFileIds.Length > 0)
            {
                // Set deleted physical file
                var filesToRemove = fileIds.Except(request.Input.Details.PhysicalFileIds).ToArray();
                foreach (var fileId in filesToRemove)
                {
                    _dbContext.PhysicalFiles
                        .Where(x => x.PhysicalFileId == fileId)
                        .UpdateFromQuery(x => new PhysicalFiles
                        {
                            Deleted = true,
                            UpdatedAt = DateTime.UtcNow,
                            UpdatedBy = request.UserId.ToString()
                        });
                }

                // Insert new file and set page number
                foreach (var inputFileId in request.Input.Details.PhysicalFileIds)
                {
                    _dbContext.PhysicalFiles
                        .Where(x => x.PhysicalFileId == inputFileId)
                        .UpdateFromQuery(x => new PhysicalFiles
                        {
                            DocumentId = request.Input.DocumentId,
                            PageNumber = Array.IndexOf(request.Input.Details.PhysicalFileIds, inputFileId) + 1,
                            UpdatedAt = DateTime.UtcNow,
                            UpdatedBy = request.UserId.ToString()
                        });
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _notificationService.TriggerUpdateDocument(request.Input.DocumentId, cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

