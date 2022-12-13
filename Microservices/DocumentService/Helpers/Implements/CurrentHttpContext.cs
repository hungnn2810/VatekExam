using System;
using DocumentService.Commons.Constants;
using DocumentService.Commons.Enums;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Helpers.Implements
{
    public class CurrentHttpContext : ICurrentHttpContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid? _userId;
        private UserTypeEnum? _userTypeId;

        public Guid? UserId
        {
            get
            {
                if (_userId.HasValue)
                    return _userId;

                var userId = GetClaimValue(OAuthConstants.ClaimTypes.UserId);
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }
                else
                {
                    _userId = Guid.Parse(userId);
                    return _userId;
                }
            }
        }

        public UserTypeEnum? UserTypeId
        {
            get
            {
                if (_userTypeId.HasValue)
                    return _userTypeId;

                var userType = GetClaimValue(OAuthConstants.ClaimTypes.UserType);
                if (string.IsNullOrEmpty(userType) || !int.TryParse(userType, out int userTypeId))
                {
                    return null;
                }

                _userTypeId = (UserTypeEnum)userTypeId;
                return _userTypeId;
            }
        }

        private string GetClaimValue(string claimType)
        {
            var claim = _httpContextAccessor.HttpContext.User.FindFirst(claimType);

            return claim.Value ?? string.Empty;
        }
    }
}

