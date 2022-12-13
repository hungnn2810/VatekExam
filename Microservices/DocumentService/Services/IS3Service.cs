using System;
using Amazon.S3.Model;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentService.Services
{
    public interface IS3Service
    {
        string GetPresignedUploadUrl(string bucketName, string fileKey);
        Task<GetObjectResponse> GetStat(string bucketName, string fileKey, CancellationToken cancellationToken);
        string GetTempPublicUrl(string bucketName, string fileKey);
        Task<DeleteObjectsResponse> DeleteObjects(string bucketName, string[] fileKeys, CancellationToken cancellationToken);
    }
}

