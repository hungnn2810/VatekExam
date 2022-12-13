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

        [Authorize("UserOnly")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] DocumentCreateInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        [AllowAnonymous]
        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(ApiResponseModel<DocumentDetailsResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetails([FromRoute] long documentId)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(new DocumentGetDetailsInputModel
            {
                DocumentId = documentId
            }));
        }
    }
}

