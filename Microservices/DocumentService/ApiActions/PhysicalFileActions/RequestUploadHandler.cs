using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Document.EntityFramework;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.PhysicalFiles;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Commons.Constants;
using DocumentService.Services;
using MediatR;

namespace DocumentService.ApiActions.PhysicalFileActions
{
    public class RequestUploadHandler : IRequestHandler<ApiActionAuthenticateRequest<PhysicalFileRequestUploadInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly IS3Service _s3Service;

        public RequestUploadHandler(DocumentDbContext dbContext, IS3Service s3Service)
        {
            _dbContext = dbContext;
            _s3Service = s3Service;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<PhysicalFileRequestUploadInputModel> request, CancellationToken cancellationToken)
        {
            var ext = request.Input.FileName.Split('.').Last().ToLower();

            if (!FileConstants.AllowFileExtensions.Contains(ext))
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.InvalidFileExtension);
            }

            var physicalFile = new PhysicalFiles
            {
                PhysicalFileName = request.Input.FileName,
                PhysicalFileExtention = ext,
                FileLengthInBytes = request.Input.FileLength,
                S3BucketId = AppSettingConstants.S3Settings.BucketId,
                S3FileKey = FileConstants.GetFileKey(ext),
                Active = false,
                CreatedBy = request.UserId.ToString(),
            };
            _dbContext.PhysicalFiles.Add(physicalFile);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var presignedUrl = _s3Service.GetPresignedUploadUrl(AppSettingConstants.S3Settings.BucketName, physicalFile.S3FileKey);

            return ApiResponse.CreateModel(new PhysicalFileRequestUploadResponseModel
            {
                PhysicalFileId = physicalFile.PhysicalFileId,
                PresignedUploadUrl = presignedUrl,
            });
        }
    }
}

