using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Enums;
using Application.Common.Utils;
using Identity.EntityFramework;
using IdentityService.Commands;
using IdentityService.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers.CHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ConfirmationCodeResponse>
    {
        private readonly IdentityDbContext _dbContext;

        public RegisterCommandHandler(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ConfirmationCodeResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check email existed
            var existedEmail = await _dbContext.Users
                .AnyAsync(x => x.Email == request.Email && !x.Deleted, cancellationToken);

            if (existedEmail)
            {
                return new ConfirmationCodeResponse
                {
                    Success = false,
                    Messages = "Email already existed"
                };
            }

            var existedUserName = await _dbContext.Users
                .AnyAsync(x => x.UserName == request.UserName && !x.Deleted, cancellationToken);

            if (existedUserName)
            {
                return new ConfirmationCodeResponse
                {
                    Success = false,
                    Messages = "User name already existed"
                };
            }

            // Add new user
            var user = new Users
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                PasswordHashed = StringHelper.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserStatusId = (short)UserStatusEnum.WaitForVerify,
                CreatedAt = DateTime.Now,
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create confirmation code


            var verifyCode = new Random().Next(100000, 999999);

            do
            {

            }
            while (CheckExistedCode(verifyCode.ToString()).Result);

            _dbContext.ConfirmationCodes.Add(new ConfirmationCodes
            {
                ConfirmationCodeId = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                ConfirmationCode = verifyCode.ToString(),
                ExpiredTime = DateTime.Now.AddHours(1),
            });
            await _dbContext.SaveChangesAsync();

            return new ConfirmationCodeResponse
            {
                Success = true,
                ConfirmationCode = verifyCode.ToString()
            };
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

