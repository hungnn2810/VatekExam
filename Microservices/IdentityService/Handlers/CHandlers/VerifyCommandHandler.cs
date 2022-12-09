using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Enums;
using Identity.EntityFramework;
using IdentityService.Commands;
using IdentityService.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers.CHandlers
{
    public class VerifyCommandHandler : IRequestHandler<VerifyCommand, BaseResponse>
    {
        private readonly IdentityDbContext _dbContext;

        public VerifyCommandHandler(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> Handle(VerifyCommand request, CancellationToken cancellationToken)
        {
            var confirmation = await _dbContext.ConfirmationCodes
                .Where(x => x.ConfirmationCodeId == request.ConfirmationCodeId.ToString() &&
                    x.ConfirmationCode == request.ConfirmationCode)
                .FirstOrDefaultAsync(cancellationToken);

            if (confirmation == null || confirmation.ConfirmationCode != request.ConfirmationCode)
            {
                return new BaseResponse
                {
                    Success = false,
                    Messages = "Invalid confirmation code"
                };
            }

            if (confirmation.ExpiredTime < DateTime.Now)
            {
                return new BaseResponse
                {
                    Success = false,
                    Messages = "Confirmation code expired"
                };
            }

            // Check user existed
            var user = await _dbContext.Users
                .Where(x => x.UserId == confirmation.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {

            }

            if (user.UserStatusId == (short)UserStatusEnum.WaitForVerify)
            {

            }

            user.UserStatusId = (short)UserStatusEnum.Normal;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.UserId;

            return new BaseResponse
            {
                Success = true
            };
        }
    }
}

