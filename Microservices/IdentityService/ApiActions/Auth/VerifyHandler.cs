using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Identity.EntityFramework;
using IdentityService.ApiModel.ApiErrorMessages;
using IdentityService.ApiModels.ApiInputModels.Auth;
using IdentityService.Commoms.Enums;
using IdentityService.Commons.Communication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers.CHandlers
{
    public class VerifyHandler : IRequestHandler<ApiActionAnonymousRequest<AuthVerifyInputModel>, IApiResponse>
    {
        private readonly IdentityDbContext _dbContext;

        public VerifyHandler(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<AuthVerifyInputModel> request, CancellationToken cancellationToken)
        {
            var confirmation = await _dbContext.ConfirmationCodes
                .Where(x => x.ConfirmationCodeId == request.Input.ConfirmationCodeId.ToString())
                .FirstOrDefaultAsync(cancellationToken);

            if (confirmation == null || confirmation.ConfirmationCode != request.Input.ConfirmationCode)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.InvalidConfirmationCode);
            }

            if (confirmation.ExpiredTime < DateTime.Now)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.ConfirmationCodeExpired);
            }

            // Check user existed
            var user = await _dbContext.Users
                .Where(x => x.UserId == confirmation.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.UserNotFound);
            }

            if (user.UserStatusId != (short)UserStatusEnum.WaitForVerify)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.UserAlreadyVerified);
            }

            user.UserStatusId = (short)UserStatusEnum.Normal;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.UserId;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(HttpStatusCode.OK);
        }
    }
}

