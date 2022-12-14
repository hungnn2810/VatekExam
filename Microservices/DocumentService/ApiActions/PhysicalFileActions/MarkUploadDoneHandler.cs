using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.PhysicalFiles;
using DocumentService.Commons.Communication;
using DocumentService.Services;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.PhysicalFileActions
{
    public class MarkUploadDoneHandler : IRequestHandler<ApiActionAuthenticateRequest<PhysicalFileMarkUploadDoneInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly IS3Service _s3Service;

        public MarkUploadDoneHandler(DocumentDbContext dbContext, IS3Service s3Service)
        {
            _dbContext = dbContext;
            _s3Service = s3Service;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<PhysicalFileMarkUploadDoneInputModel> request, CancellationToken cancellationToken)
        {
            var files = await _dbContext.PhysicalFiles
                  .Where(pf => !pf.Deleted && request.Input.PhysicalFileIds.Contains(pf.PhysicalFileId))
                  .Select(pf => new { pf, pf.S3Bucket })
                  .ToArrayAsync(cancellationToken);

            if (files.Length != request.Input.PhysicalFileIds.Length)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.PhysicalFileNotFound);
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            // Check file length
            foreach (var file in files)
            {
                var fileStat = await _s3Service.GetStat(file.S3Bucket.S3BucketName, file.pf.S3FileKey, cancellationToken);
                if (fileStat == null || fileStat.ContentLength != file.pf.FileLengthInBytes)
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.FileCorrupted);
                }

                file.pf.Active = true;
                file.pf.UpdatedBy = request.UserId.ToString();
                file.pf.UpdatedAt = System.DateTime.UtcNow;
                _dbContext.PhysicalFiles.Update(file.pf);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

