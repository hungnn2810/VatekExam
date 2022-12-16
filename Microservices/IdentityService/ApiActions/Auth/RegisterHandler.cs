using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Identity;
using IdentityService.ApiModel.ApiErrorMessages;
using IdentityService.ApiModels.ApiInputModels.Auth;
using IdentityService.ApiModels.ApiResponseModels;
using IdentityService.Commoms.Enums;
using IdentityService.Commons.Communication;
using IdentityService.Commons.Enums;
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
                .AnyAsync(x => x.Email == request.Input.Email, cancellationToken);

            if (existedEmail)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.EmailAlreadyExisted);
            }

            var existedUserName = await _dbContext.Users
                .AnyAsync(x => x.UserName == request.Input.UserName, cancellationToken);

            if (existedUserName)
            {
                return ApiResponse.CreateErrorModel(HttpStatusCode.BadRequest, ApiInternalErrorMessages.UserNameAlreadyExisted);
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

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
                UserTypeId = (short)UserTypeEnum.User,
                CreatedAt = DateTime.Now,
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create confirmation code
            var verifyCode = new Random().Next(100000, 999999);
            var confirmationCodeId = Guid.NewGuid();
            var newConfirmationCode = new ConfirmationCodes
            {
                ConfirmationCodeId = confirmationCodeId.ToString(),
                UserId = user.UserId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = DateTime.UtcNow.AddMinutes(10),
            };
            _dbContext.ConfirmationCodes.Add(newConfirmationCode);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync(cancellationToken);

            return ApiResponse.CreateModel(new ConfirmationResponseModel
            {
                ConfirmationCodeId = confirmationCodeId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = newConfirmationCode.ExpiredTime
            });
        }
    }

}

