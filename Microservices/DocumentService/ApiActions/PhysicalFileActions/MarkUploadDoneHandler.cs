using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Document.EntityFramework;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.PhysicalFiles;
using DocumentService.Commons.Communication;
using DocumentService.Services;
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
            var file = await _dbContext.PhysicalFiles
                  .Where(pf => !pf.Deleted && pf.PhysicalFileId == request.Input.PhysicalFileId)
                  .Select(pf => new { pf, pf.S3Bucket })
                  .FirstOrDefaultAsync(cancellationToken);

            if (file == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.PhysicalFileNotFound);
            }

            var fileStat = await _s3Service.GetStat(file.S3Bucket.S3BucketName, file.pf.S3FileKey, cancellationToken);
            if (fileStat == null || fileStat.ContentLength != file.pf.FileLengthInBytes)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.FileCorrupted);
            }

            file.pf.Active = true;
            _dbContext.PhysicalFiles.Update(file.pf);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

