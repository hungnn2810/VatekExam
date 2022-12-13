using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Document.EntityFramework;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.Commons.Communication;
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
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DuplicatedDocumentName);
            }

            var existFile = await _dbContext.PhysicalFiles
                .AnyAsync(x => !x.Deleted && x.Active &&
                    x.PhysicalFileId == request.Input.PhysicalFileId, cancellationToken);

            if (!existFile)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.PhysicalFileNotFound);
            }
            #endregion

            _dbContext.Documents.Add(new Documents
            {
                CategoryId = request.Input.CategoryId,
                Title = request.Input.Title,
                AuthorId = request.UserId.ToString(),
                PhysicalFileId = request.Input.PhysicalFileId,
                CreatedBy = request.UserId.ToString()
            });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

