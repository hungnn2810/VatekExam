using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Bookmarks;
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
    }
}