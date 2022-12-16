using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.PhysicalFiles;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Filters;
using DocumentService.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("physical-files")]
    public class PhysicalFileController : BaseController
    {
        public PhysicalFileController(IMediator mediator,
            ICurrentHttpContext currentHttpContext)
        : base(mediator, currentHttpContext)
        {
        }

        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [MultiplePoliciesAuthorize("AdminOnly;UserOnly")]
        [HttpPost("")]
        [ProducesResponseType(typeof(ApiResponseModel<PhysicalFileUploadResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestUpload([FromForm] PhysicalFileUploadInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }
    }
}

