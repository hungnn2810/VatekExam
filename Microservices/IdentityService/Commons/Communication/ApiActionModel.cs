using System;
using MediatR;

namespace IdentityService.Commons.Communication
{
    public class ApiActionModel
    {
        public static ApiActionAnonymousRequest<T> CreateRequest<T>(T input)
            where T : IApiInput, new()
        {
            return new ApiActionAnonymousRequest<T>()
            {
                Input = input
            };
        }

        public static ApiActionAuthenticateRequest<T> CreateRequest<T>(Guid userId, T input)
            where T : IApiInput, new()
        {
            return new ApiActionAuthenticateRequest<T>()
            {
                Input = input,
                UserId = userId
            };
        }
    }

    public class ApiActionAnonymousRequest<T> : IRequest<IApiResponse>
    {
        public ApiActionAnonymousRequest()
        { }

        public T Input { get; set; }
    }

    public class ApiActionAuthenticateRequest<T> : IRequest<IApiResponse>
    {
        public ApiActionAuthenticateRequest()
        { }

        public T Input { get; set; }
        public Guid UserId { get; set; }
    }
}

