using System;
using DocumentService.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected ICurrentHttpContext _currentHttpContext;

        public BaseController(IMediator mediator, ICurrentHttpContext currentHttpContext)
        {
            _mediator = mediator;
            _currentHttpContext = currentHttpContext;
        }

        protected Guid UserId => _currentHttpContext.UserId.Value;
    }
}

