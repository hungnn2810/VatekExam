using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Identity.EntityFramework
{
    public partial class ConfirmationCodes
    {
        public string ConfirmationCodeId { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Users User { get; set; }
    }
}
