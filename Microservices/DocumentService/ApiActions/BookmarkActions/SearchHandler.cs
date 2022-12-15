using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Bookmarks;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Commons.Enums;
using EntityFramework.Document;
using EntityFramework.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.ApiActions.BookmarkActions
{
    public class SearchHandler : IRequestHandler<ApiActionAuthenticateRequest<BookmarkSearchInputModel>, IApiResponse>
    {
        private readonly DocumentDbContext _documentContext;
        private readonly IdentityDbContext _identityContext;

        public SearchHandler(DocumentDbContext documentContext, IdentityDbContext identityContext)
        {
            _documentContext = documentContext;
            _identityContext = identityContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAuthenticateRequest<BookmarkSearchInputModel> request, CancellationToken cancellationToken)
        {
            var query = from bm in _documentContext.Bookmarks
                        where !bm.Document.Deleted &&
                        bm.Document.Visible.Value &&
                        bm.UserId == request.UserId.ToString() &&
                        (request.Input.CategoryId == null || !request.Input.CategoryId.HasValue ||
                           bm.Document.CategoryId == request.Input.CategoryId.Value)
                        select new
                        {
                            bm.BookmarkId,
                            bm.DocumentId,
                            bm.PageNumber,
                            bm.Document.Title,
                            bm.Document.Category.CategoryName,
                            bm.Document.AuthorId,
                            bm.Document.Visible,
                            bm.Document.CreatedAt,
                            bm.Document.UpdatedAt
                        };

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
                .Where(x => !x.Deleted &&
                    x.UserStatusId == (short)UserStatusEnum.Normal &&
                    result.Select(x => x.AuthorId).Contains(x.UserId))
                .Select(x => new
                {
                    x.UserId,
                    FullName = $"{x.FirstName} {x.LastName}"
                })
                .ToListAsync(cancellationToken);

            return ApiResponse.CreatePagingModel(
                result.Select(x => new BookmarkResponseModel
                {
                    BookmarkId = x.BookmarkId,
                    DocumentId = x.DocumentId,
                    PageNumber = x.PageNumber,
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

