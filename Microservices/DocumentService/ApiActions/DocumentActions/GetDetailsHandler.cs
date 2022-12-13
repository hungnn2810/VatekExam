using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Document.EntityFramework;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Commons.Enums;
using DocumentService.Services;
using Identity.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class GetDetailsHandler : IRequestHandler<ApiActionAnonymousRequest<DocumentGetDetailsInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _documentContext;
        private readonly IdentityDbContext _identityContext;
        private readonly IS3Service _s3Service;

        public GetDetailsHandler(
            DocumentDbContext documentContext,
            IdentityDbContext identityContext,
            IS3Service s3Service)
        {
            _documentContext = documentContext;
            _identityContext = identityContext;
            _s3Service = s3Service;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<DocumentGetDetailsInputModel> request, CancellationToken cancellationToken)
        {
            var document = await (from d in _documentContext.Documents
                                  where !d.Deleted && d.DocumentId == request.Input.DocumentId
                                  select new
                                  {
                                      d.Category.CategoryName,
                                      d.Title,
                                      d.AuthorId,
                                      FileUrl = _s3Service.GetTempPublicUrl(d.PhysicalFile.S3Bucket.S3BucketName, d.PhysicalFile.S3FileKey)
                                  }).FirstOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.OK, ApiInternalErrorMessages.DocumentNotFound);
            }

            var author = await _identityContext.Users
                .Where(x => !x.Deleted &&
                    x.UserId == document.AuthorId &&
                    x.UserStatusId == (short)UserStatusEnum.Normal)
                .Select(x => new { x.FirstName, x.LastName })
                .FirstOrDefaultAsync(cancellationToken);

            return ApiResponse.CreateModel(new DocumentDetailsResponseModel
            {
                CategoryName = document.CategoryName,
                Title = document.Title,
                AuthorName = $"{author.LastName} {author.FirstName}",
                FileUrl = document.FileUrl
            });
        }
    }
}

