using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("documents")]
    [Produces("application/json")]
    public class DocumentController : BaseController
    {
        public DocumentController(IMediator mediator,
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
        public async Task<IActionResult> Create([FromBody] DocumentCreateInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpDelete("{documentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromRoute] long documentId)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentDeleteInputModel
            {
                DocumentId = documentId
            }));
        }

        /// <summary>
        /// Set visible or invisible
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpPut("{documentId}/set-visible")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetVisible([FromRoute] long documentId, [FromBody] DocumentSetVisibleInput details)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentSetVisibleInputModel
            {
                DocumentId = documentId,
                Details = details
            }));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpPut("{documentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] long documentId, [FromBody] DocumentUpdateInput details)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentUpdateInputModel
            {
                DocumentId = documentId,
                Details = details
            }));
        }

        /// <summary>
        /// Search my documents
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpGet("my-documents")]
        [ProducesResponseType(typeof(ApiPagingResponseModel<DocumentResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchMyDocument(
            [Range(1, 100)] int pageSize = 50,
            [Range(1, int.MaxValue)] int pageNumber = 1,
            [StringLength(128)] string keyword = "")
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentSearchForOwnerInputModel
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Keyword = keyword
            }));
        }
    }
}

