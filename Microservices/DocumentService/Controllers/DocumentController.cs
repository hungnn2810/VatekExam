using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Documents;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Filters;
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
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseModel<DocumentResponseModel>),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] DocumentCreateInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
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
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
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
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
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
        /// Update content
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
        [HttpPatch("{documentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateContent([FromRoute] long documentId, [FromBody] DocumentUpdateContentInput details)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentUpdateContentInputModel
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
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
        [HttpGet("my-documents")]
        [ProducesResponseType(typeof(ApiPagingResponseModel<DocumentResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchMyDocument(
            [Range(1, 100)] int pageSize = 50,
            [Range(1, int.MaxValue)] int pageNumber = 1,
            [StringLength(128)] string keyword = "",
            [FromQuery] long? categoryId = null)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentSearchForOwnerInputModel
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Keyword = keyword,
                CategoryId = categoryId
            }));
        }

        /// <summary>
        /// Get details for owner
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(ApiResponseModel<DocumentDetailsResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetails([FromRoute] long documentId)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new DocumentGetDetailsInputModel
            {
                DocumentId = documentId
            }));
        }

        /// <summary>
        /// Get page content
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("content")]
        [ProducesResponseType(typeof(ApiResponseModel<DocumentPageRepsonseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPageContent([FromQuery] long documentId, [FromQuery] int pageNumber)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(new DocumentGetPageContentInputModel
            {
                DocumentId = documentId,
                PageNumber = pageNumber
            }));
        }

        /// <summary>
        /// Search for anonymous
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiPagingResponseModel<DocumentResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Search(
            [Range(1, 100)] int pageSize = 50,
            [Range(1, int.MaxValue)] int pageNumber = 1,
            [StringLength(128)] string keyword = "",
            [FromQuery] long? categoryId = null)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(new DocumentSearchInputModel
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Keyword = keyword,
                CategoryId = categoryId
            }));
        }
    }
}

