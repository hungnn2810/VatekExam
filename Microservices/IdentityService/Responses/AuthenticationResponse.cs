﻿namespace IdentityService.Responses
{
    public class AuthenticationResponse : BaseResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
	}
}

