using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.PhysicalFiles;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Commons.Constants;
using DocumentService.Services;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.PhysicalFileActions
{
    public class MarkUploadDoneHandler : IRequestHandler<ApiActionAuthenticateRequest<PhysicalFileUploadInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly IS3Service _s3Service;

        public MarkUploadDoneHandler(DocumentDbContext dbContext, IS3Service s3Service)
        {
            _dbContext = dbContext;
            _s3Service = s3Service;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<PhysicalFileUploadInputModel> request, CancellationToken cancellationToken)
        {
            #region Validate input
            foreach (var file in request.Input.Files)
            {
                var ext = file.FileName.Split('.').Last().ToLower();

                if (!FileConstants.AllowFileExtensions.Contains(ext))
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.InvalidFileExtension);
                }

                if (file.Length >= FileConstants.MaxSize)
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.FileTooLarge);
                }
            }
            #endregion

            List<long> physicalFileIds = new();
            foreach (var file in request.Input.Files)
            {
                var ext = file.FileName.Split('.').Last().ToLower();

                var physicalFile = new PhysicalFiles
                {
                    PhysicalFileName = file.FileName,
                    PhysicalFileExtention = ext,
                    FileLengthInBytes = file.Length,
                    S3BucketId = AppSettingConstants.S3Settings.BucketId,
                    S3FileKey = FileConstants.GetFileKey(ext),
                    Active = false,
                    Deleted = false,
                    CreatedBy = request.UserId.ToString(),
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.PhysicalFiles.Add(physicalFile);
                await _dbContext.SaveChangesAsync(cancellationToken);

                physicalFileIds.Add(physicalFile.PhysicalFileId);
                var uploadResponse = await _s3Service.UploadFile(AppSettingConstants.S3Settings.BucketName, physicalFile.S3FileKey, file, cancellationToken);

                if (uploadResponse)
                {
                    physicalFile.Active = true;
                    physicalFile.UpdatedBy = request.UserId.ToString();
                    physicalFile.UpdatedAt = DateTime.UtcNow;
                    _dbContext.PhysicalFiles.Update(physicalFile);
                }
                else
                {
                    return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.UploadFailed);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(new PhysicalFileUploadResponseModel
            {
                PhysicalFileIds = physicalFileIds.ToArray()
            });
        }
    }
}

