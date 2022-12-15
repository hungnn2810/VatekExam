using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Bookmarks;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("bookmarks")]
    [Produces("applications/json")]
    public class BookmarkController : BaseController
    {
        public BookmarkController(IMediator mediator,
            ICurrentHttpContext currentHttpContext)
        : base(mediator, currentHttpContext)
        {
        }

        /// <summary>
        /// Create new
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] BookmarkCreateInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        /// <summary>
        /// Search by document title
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiPagingResponseModel<BookmarkResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Search(
            [Range(1, 100)] int pageSize = 50,
            [Range(1, int.MaxValue)] int pageNumber = 1,
            [StringLength(128)] string keyword = "",
            [FromQuery] long? categoryId = null)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new BookmarkSearchInputModel
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Keyword = keyword,
                CategoryId = categoryId
            }));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="bookmarkId"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpDelete("{bookmarkId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromRoute] long bookmarkId)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new BookmarkDeleteInputModel
            {
                BookmarkId = bookmarkId
            }));
        }
    }
}