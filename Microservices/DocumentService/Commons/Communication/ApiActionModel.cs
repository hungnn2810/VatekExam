using System;
using MediatR;

namespace DocumentService.Commons.Communication
{
    public class ApiActionModel
    {
        public static ApiActionAnonymousRequest<T> CreateRequest<T>(T input)
            where T : IApiInput, new()
        {
            return new ApiActionAnonymousRequest<T>
            {
                Input = input
            };
        }

        public static ApiActionAuthenticateRequest<T> CreateRequest<T>(Guid userId, T input)
        {
            return new ApiActionAuthenticateRequest<T>
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

        public Guid UserId { get; set; }
        public T Input { get; set; }
    }
}

