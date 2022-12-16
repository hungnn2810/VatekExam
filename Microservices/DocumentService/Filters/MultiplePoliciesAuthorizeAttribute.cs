using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DocumentService.Filters
{
    public class MultiplePoliciesAuthorizeAttribute : TypeFilterAttribute
    {
        public MultiplePoliciesAuthorizeAttribute(string policys, bool isAnd = false) : base(typeof(MultiplePoliciesAuthorizeFilter))
        {
            Arguments = new object[] { policys, isAnd };
        }
    }

    public class MultiplePoliciesAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        public string Policys { get; private set; }
        public bool IsAnd { get; private set; }

        public MultiplePoliciesAuthorizeFilter(string policys, bool isAnd, IAuthorizationService authorization)
        {
            Policys = policys;
            IsAnd = isAnd;
            _authorization = authorization;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var policys = Policys.Split(";").ToList();
            if (IsAnd)
            {
                foreach (var policy in policys)
                {
                    var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (!authorized.Succeeded)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }
            else
            {
                foreach (var policy in policys)
                {
                    var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (authorized.Succeeded)
                    {
                        return;
                    }

                }
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}

