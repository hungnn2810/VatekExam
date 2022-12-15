using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.Categories;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("categories")]
    [Produces("application/json")]
    public class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator,
            ICurrentHttpContext currentHttpContext)
        : base(mediator, currentHttpContext)
        {
        }

        /// <summary>
        /// Create new
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize("AdminOnly")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        /// <summary>
        /// Get all visible
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiArrayResponseModel<CategoryResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(new CategoryGetAllInputModel
            {
            }));
        }

        /// <summary>
        /// Search visible and invisible
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Authorize("AdminOnly")]
        [HttpGet("admin")]
        [ProducesResponseType(typeof(ApiPagingResponseModel<CategoryResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Search(
            [Range(1, 100)] int pageSize = 50,
            [Range(1, int.MaxValue)] int pageNumber = 1,
            [StringLength(128)] string keyword = "")
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new CategorySearchInputModel
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Keyword = keyword
            }));
        }

        /// <summary>
        /// Set visible or invisible
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [Authorize("AdminOnly")]
        [HttpPut("{categoryId}/set-visible")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetVisible([FromRoute] long categoryId, [FromBody] CategorySetVisibleInput details)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new CategorySetVisibleInputModel
            {
                CategoryId = categoryId,
                Details = details
            }));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [Authorize("AdminOnly")]
        [HttpPut("{categoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] long categoryId, [FromBody] CategoryUpdateInput details)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new CategoryUpdateInputModel
            {
                CategoryId = categoryId,
                Details = details
            }));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Authorize("AdminOnly")]
        [HttpDelete("{categoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromRoute] long categoryId)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, new CategoryDeleteInputModel
            {
                CategoryId = categoryId
            }));
        }
    }
}

