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
    public class DeleteHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentDeleteInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly INotificationService _notificationService;

        public DeleteHandler(DocumentDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentDeleteInputModel> request, CancellationToken cancellationToken)
        {
            var document = await _dbContext.Documents
                .Where(x => x.AuthorId == request.UserId.ToString() &&
                    x.DocumentId == request.Input.DocumentId)
                .FirstOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DocumentNotFound);
            }

            var files = await _dbContext.PhysicalFiles
                .Where(x => x.DocumentId == request.Input.DocumentId)
                .ToArrayAsync(cancellationToken);

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            document.Deleted = true;
            document.UpdatedBy = request.UserId.ToString();
            document.UpdatedAt = System.DateTime.UtcNow;
            _dbContext.Documents.Update(document);

            // Set deleted physical file
            foreach (var file in files)
            {
                file.Deleted = true;
                file.UpdatedAt = DateTime.UtcNow;
                file.UpdatedBy = request.UserId.ToString();
                _dbContext.PhysicalFiles.Update(file);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _notificationService.TriggerDeleteDocument(request.Input.DocumentId, cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

