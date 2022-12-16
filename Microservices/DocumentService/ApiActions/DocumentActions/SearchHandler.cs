using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Commons.Enums;
using EntityFramework.Document;
using EntityFramework.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.DocumentActions
{
    public class SearchHandler : IRequestHandler<ApiActionAnonymousRequest<DocumentSearchInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _documentContext;
        private readonly IdentityDbContext _identityContext;

        public SearchHandler(DocumentDbContext documentContext, IdentityDbContext identityContext)
        {
            _documentContext = documentContext;
            _identityContext = identityContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<DocumentSearchInputModel> request, CancellationToken cancellationToken)
        {
            var query = from d in _documentContext.Documents
                         where d.Visible
                         select new
                         {
                             d.DocumentId,
                             d.CategoryId,
                             d.Title,
                             d.Category.CategoryName,
                             d.AuthorId,
                             d.Visible,
                             d.CreatedAt,
                             d.UpdatedAt
                         };

            if (request.Input.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == request.Input.CategoryId);
            }

            if (!string.IsNullOrEmpty(request.Input.Keyword) && request.Input.Keyword.Length >= 2)
            {
                query = from item in query
                        where item.Title.Contains(request.Input.Keyword)
                        select item;
            }

            var totalItems = await query.CountAsync(cancellationToken);
            var requestPaging = new ApiResponsePaging(request.Input.PageSize, request.Input.PageNumber, totalItems);

            var result = await query
                .Skip(totalItems * (requestPaging.PageNumber - 1))
                .Take(requestPaging.PageSize)
                .ToListAsync(cancellationToken);

            var authors = await _identityContext.Users
                .Where(x => x.UserStatusId == (short)UserStatusEnum.Normal &&
                    result.Select(x => x.AuthorId).Contains(x.UserId))
                .Select(x => new
                {
                    x.UserId,
                    FullName = $"{x.FirstName} {x.LastName}"
                })
                .ToListAsync(cancellationToken);

            return ApiResponse.CreatePagingModel(
                result.Select(x => new DocumentResponseModel
                {
                    DocumentId = x.DocumentId,
                    Title = x.Title,
                    CategoryName = x.CategoryName,
                    Author = authors.Where(u => u.UserId == x.AuthorId).Select(u => u.FullName).FirstOrDefault(),
                    Visible = x.Visible,
                    CreatedAtUtc = x.CreatedAt,
                    UpdatedAtUtc = x.UpdatedAt
                }).ToArray(),
                requestPaging);
        }
    }
}

