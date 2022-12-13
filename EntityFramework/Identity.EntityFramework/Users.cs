using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Identity.EntityFramework
{
    public partial class Users
    {
        public Users()
        {
            ConfirmationCodes = new HashSet<ConfirmationCodes>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHashed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public short? UserTypeId { get; set; }
        public short UserStatusId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual UserStatuses UserStatus { get; set; }
        public virtual UserTypes UserType { get; set; }
        public virtual ICollection<ConfirmationCodes> ConfirmationCodes { get; set; }
    }
}
