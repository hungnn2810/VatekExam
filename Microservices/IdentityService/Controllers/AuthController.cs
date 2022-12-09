using System.Threading.Tasks;
using IdentityService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand input)
        {
            var result = await _mediator.Send(input);
            return result != null ? Created("", result) : BadRequest(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyCommand input)
        {
            var result = await _mediator.Send(input);
            return result != null ? Ok() : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand input)
        {
            var result = await _mediator.Send(input);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}

