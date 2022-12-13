using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using DocumentService.Commons.Constants;

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

        public string GetPresignedUploadUrl(string bucketName, string fileKey)
        {
            var request = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.Now.AddMinutes(60 * 60)
            });

            return request;
        }

        public async Task<GetObjectResponse> GetStat(string bucketName, string fileKey, CancellationToken cancellationToken)
        {
            try
            {
                return await _s3Client.GetObjectAsync(bucketName, fileKey, cancellationToken);
            }
            catch (AmazonS3Exception)
            {
                return null;
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

