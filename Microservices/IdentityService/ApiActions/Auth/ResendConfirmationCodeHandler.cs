using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.EntityFramework;
using IdentityService.ApiModels.ApiInputModels.Auth;
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
            //var confirmationCode = await _dbContext.ConfirmationCodes
            //    .Where(x => x.ConfirmationCodeId == request.Input.ConfirmationCodeId.ToString())
            //    .FirstOrDefaultAsync(cancellationToken);

            //if(confirmationCode)

            throw new NotImplementedException();
        }
    }
}

