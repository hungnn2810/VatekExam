using IdentityService.Responses;
using MediatR;

namespace IdentityService.Commands
{
    public class LoginCommand : IRequest<AuthenticationResponse>
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}

