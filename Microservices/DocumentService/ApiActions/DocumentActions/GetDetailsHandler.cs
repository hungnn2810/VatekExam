using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Services;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class GetDetailsHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentGetDetailsInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly IS3Service _s3Service;

        public GetDetailsHandler(DocumentDbContext dbContext, IS3Service s3Service)
        {
            _dbContext = dbContext;
            _s3Service = s3Service;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentGetDetailsInputModel> request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Documents
                .Where(x => x.AuthorId == request.UserId.ToString() &&
                    x.DocumentId == request.Input.DocumentId)
                .Select(x => new DocumentDetailsResponseModel
                {
                    DocumentId = x.DocumentId,
                    Title = x.Title,
                    CategoryId = x.CategoryId,
                    Visible = x.Visible,
                    UpdatedAtUtc = x.UpdatedAt,
                    Pages = x.PhysicalFiles
                        .Where(pf => pf.Active)
                        .Select(pf => new DocumentPageRepsonseModel
                        {
                            PhysicalFileId = pf.PhysicalFileId,
                            FileUrl = _s3Service.GetTempPublicUrl(pf.S3Bucket.S3BucketName, pf.S3FileKey),
                            PageNumber = pf.PageNumber.Value
                        }).ToArray()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DocumentNotFound);
            }

            return ApiResponse.CreateModel(data);
        }
    }
}

