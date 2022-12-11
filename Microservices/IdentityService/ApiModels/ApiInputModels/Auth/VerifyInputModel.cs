﻿using System;
using IdentityService.Commons.Communication;

namespace IdentityService.ApiModels.ApiInputModels.Auth
{
    public class VerifyInputModel : IApiInput
    {
        public Guid ConfirmationCodeId { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
