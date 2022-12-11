using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Document.EntityFramework
{
    public partial class BookMarks
    {
        public long BookMarkId { get; set; }
        public long? DocumentId { get; set; }
        public string UserId { get; set; }
        public int PageNumber { get; set; }

        public virtual Documents Document { get; set; }
    }
}
