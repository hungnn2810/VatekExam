using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiErrorMessages;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.Commons.Communication;
using EntityFramework.Document;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class SetVisibleHandler : IRequestHandler<ApiActionAuthenticateRequest<DocumentSetVisibleInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _dbContext;

        public SetVisibleHandler(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<DocumentSetVisibleInputModel> request, CancellationToken cancellationToken)
        {
            var document = await _dbContext.Documents
               .Where(x => x.Visible != request.Input.Details.Visible &&
                   x.AuthorId == request.UserId.ToString() &&
                   x.DocumentId == request.Input.DocumentId)
               .FirstOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.DocumentNotFound);
            }

            document.Visible = request.Input.Details.Visible;
            document.UpdatedAt = System.DateTime.UtcNow;
            document.UpdatedBy = request.UserId.ToString();
            _dbContext.Documents.Update(document);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

