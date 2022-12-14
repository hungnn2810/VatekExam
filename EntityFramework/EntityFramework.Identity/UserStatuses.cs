using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EntityFramework.Identity
{
    public partial class UserStatuses
    {
        public UserStatuses()
        {
            Users = new HashSet<Users>();
        }

        public short UserStatusId { get; set; }
        public string UserStatusName { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
