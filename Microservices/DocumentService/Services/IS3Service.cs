using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Services
{
    public interface IS3Service
    {
        Task<bool> UploadFile(string bucketName, string fileKey, IFormFile file, CancellationToken cancellationToken);
        string GetTempPublicUrl(string bucketName, string fileKey);
        Task<DeleteObjectsResponse> DeleteObjects(string bucketName, string[] fileKeys, CancellationToken cancellationToken);
    }
}

