using System.Linq;
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
    public class GetPageContentHandler : IRequestHandler<ApiActionAnonymousRequest<DocumentGetPageContentInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;
        private readonly IS3Service _s3Service;

        public GetPageContentHandler(DocumentDbContext dbContext, IS3Service s3Service)
        {
            _dbContext = dbContext;
            _s3Service = s3Service;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<DocumentGetPageContentInputModel> request, CancellationToken cancellationToken)
        {
            var data = await (from x in _dbContext.DocumentPages
                              where
                              !x.Document.Deleted && x.Document.Visible.Value &&
                              x.DocumentId == request.Input.DocumentId &&
                              x.PageNumber == request.Input.PageNumber
                              select new
                              {
                                  x.PhysicalFileId,
                                  x.PageNumber,
                                  x.PhysicalFile.S3Bucket.S3BucketName,
                                  x.PhysicalFile.S3FileKey
                              }).FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                return ApiResponse.CreateErrorModel(System.Net.HttpStatusCode.BadRequest, ApiInternalErrorMessages.PageContentNotFound);
            }

            return ApiResponse.CreateModel(new DocumentPageRepsonseModel
            {
                PhysicalFileId = data.PhysicalFileId,
                FileUrl = _s3Service.GetTempPublicUrl(data.S3BucketName, data.S3FileKey),
                PageNumber = data.PageNumber
            });
        }
    }
}

