using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DocumentService.Commons.Constants;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Services.Implements
{
    public class S3Service : IS3Service
    {
        private readonly AmazonS3Client _s3Client;

        public S3Service()
        {
            _s3Client = new AmazonS3Client(
                AppSettingConstants.S3Settings.AccessKey,
                AppSettingConstants.S3Settings.SecretKey,
                RegionEndpoint.GetBySystemName(AppSettingConstants.S3Settings.Endpoint));
        }

        public async Task<bool> UploadFile(string bucketName, string fileKey, IFormFile file, CancellationToken cancellationToken)
        {
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    file.CopyTo(memoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = fileKey,
                        BucketName = bucketName,
                    };

                    var fileTransferUtility = new TransferUtility(_s3Client);
                    await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

                    return true;
                }
                catch (AmazonS3Exception)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public string GetTempPublicUrl(string bucketName, string fileKey)
        {
            return _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Expires = DateTime.Now.AddMinutes(60 * 3)
            });
        }

        public Task<DeleteObjectsResponse> DeleteObjects(string bucketName, string[] fileKeys, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

