using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Identity;
using IdentityService.ApiModel.ApiErrorMessages;
using IdentityService.ApiModels.ApiInputModels.Auth;
using IdentityService.ApiModels.ApiResponseModels;
using IdentityService.Commons.Communication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.ApiActions.Auth
{
    public class ResendConfirmationHandler : IRequestHandler<ApiActionAnonymousRequest<AuthResendConfirmationInputModel>, IApiResponse>
    {
        private readonly IdentityDbContext _dbContext;

        public ResendConfirmationHandler(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<AuthResendConfirmationInputModel> request, CancellationToken cancellationToken)
        {
            var confirmationCode = await _dbContext.ConfirmationCodes
                .Where(x => x.ConfirmationCodeId == request.Input.ConfirmationCodeId.ToString())
                .FirstOrDefaultAsync(cancellationToken);

            if (confirmationCode == null)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.InvalidConfirmationCode);
            }

            // Check user existed
            var userExisted = await _dbContext.Users
                .AnyAsync(x => !x.Deleted && x.UserId == confirmationCode.UserId, cancellationToken);

            if (!userExisted)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.InvalidConfirmationCode);
            }

            // Create confirmation code
            var verifyCode = new Random().Next(100000, 999999);
            var confirmationCodeId = Guid.NewGuid();
            var newConfirmationCode = new ConfirmationCodes
            {
                ConfirmationCodeId = confirmationCodeId.ToString(),
                UserId = confirmationCode.UserId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = DateTime.UtcNow.AddMinutes(10),
            };
            _dbContext.ConfirmationCodes.Add(newConfirmationCode);

            _dbContext.ConfirmationCodes.Remove(confirmationCode);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse.CreateModel(new ConfirmationResponseModel
            {
                ConfirmationCodeId = confirmationCodeId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = newConfirmationCode.ExpiredTime
            });
        }
    }
}

