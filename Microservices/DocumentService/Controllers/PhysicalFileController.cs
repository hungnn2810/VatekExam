using System.Net;
using System.Threading.Tasks;
using DocumentService.ApiModels.ApiInputModels.PhysicalFiles;
using DocumentService.ApiModels.ApiResponseModels;
using DocumentService.Commons.Communication;
using DocumentService.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("physical-files")]
    [Produces("application/json")]
    public class PhysicalFileController : BaseController
    {
        public PhysicalFileController(IMediator mediator,
            ICurrentHttpContext currentHttpContext)
        : base(mediator, currentHttpContext)
        {
        }

        /// <summary>
        /// Request upload
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpPost("request-upload")]
        [ProducesResponseType(typeof(ApiResponseModel<PhysicalFileRequestUploadResponseModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestUpload([FromBody] PhysicalFileRequestUploadInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }

        /// <summary>
        /// Mark physical files upload done
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize("UserOnly")]
        [HttpPut("upload-done")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadDone([FromBody] PhysicalFileMarkUploadDoneInputModel input)
        {
            return await _mediator.Send(ApiActionModel.CreateRequest(UserId, input));
        }
    }
}

