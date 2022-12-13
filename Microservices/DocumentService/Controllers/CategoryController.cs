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

        [Authorize("SuperAdminOnly")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiPagingResponseModel<CategoryResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Search(
            int pageSize = 50,
            int pageNumber = 1,
            [StringLength(128)] string keyword = "")
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(new CategorySearchInputModel
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Keyword = keyword
            }));
        }
    }
}

