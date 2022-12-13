using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Identity.EntityFramework;
using IdentityService.ApiModel.ApiErrorMessages;
using IdentityService.ApiModels.ApiInputModels.Auth;
using IdentityService.ApiModels.ApiResponseModels;
using IdentityService.Commoms.Enums;
using IdentityService.Commons.Communication;
using IdentityService.Commons.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers.CHandlers
{
    public class RegisterHandler : IRequestHandler<ApiActionAnonymousRequest<AuthRegisterInputModel>, IApiResponse>
    {
        private readonly IdentityDbContext _dbContext;

        public RegisterHandler(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IApiResponse> Handle(ApiActionAnonymousRequest<AuthRegisterInputModel> request, CancellationToken cancellationToken)
        {
            // Check email existed
            var existedEmail = await _dbContext.Users
                .AnyAsync(x => x.Email == request.Input.Email && !x.Deleted, cancellationToken);

            if (existedEmail)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.EmailAlreadyExisted);
            }

            var existedUserName = await _dbContext.Users
                .AnyAsync(x => x.UserName == request.Input.UserName && !x.Deleted, cancellationToken);

            if (existedUserName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.UserNameAlreadyExisted);
            }

            // Add new user
            var user = new Users
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = request.Input.UserName,
                PasswordHashed = StringHelper.HashPassword(request.Input.Password),
                FirstName = request.Input.FirstName,
                LastName = request.Input.LastName,
                Email = request.Input.Email,
                UserStatusId = (short)UserStatusEnum.WaitForVerify,
                CreatedAt = DateTime.Now,
                UserTypeId = 1
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create confirmation code

            var verifyCode = new Random().Next(100000, 999999);
            do
            {

            }
            while (CheckExistedCode(verifyCode.ToString()).Result);

            var confirmationCodeId = Guid.NewGuid();
            var newConfirmationCode = new ConfirmationCodes
            {
                ConfirmationCodeId = confirmationCodeId.ToString(),
                UserId = user.UserId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = DateTime.Now.AddHours(1),
            };
            _dbContext.ConfirmationCodes.Add(newConfirmationCode);
            await _dbContext.SaveChangesAsync();

            return ApiResponse.CreateModel(new ConfirmationResponseModel
            {
                ConfirmationCodeId = confirmationCodeId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = newConfirmationCode.ExpiredTime
            });
        }

        private async Task<bool> CheckExistedCode(string confirmationCode)
        {
            var res = await _dbContext.ConfirmationCodes
                .AnyAsync(x => x.ConfirmationCode == confirmationCode &&
                    x.ExpiredTime < DateTime.Now);

            return res;
        }
    }

}

