using System;
using DocumentService.Commons.Enums;

namespace DocumentService.Helpers
{
    public interface ICurrentHttpContext
    {
        Guid? UserId { get; }
        UserTypeEnum? UserTypeId { get; }
    }
}

