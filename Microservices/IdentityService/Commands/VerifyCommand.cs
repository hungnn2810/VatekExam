using System;
using IdentityService.Responses;
using MediatR;

namespace IdentityService.Commands
{
    public class VerifyCommand : IRequest<BaseResponse>
    {
        public Guid ConfirmationCodeId { get; set; }
        public string ConfirmationCode { get; set; }
    }
}

