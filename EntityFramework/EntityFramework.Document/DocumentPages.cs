using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EntityFramework.Document
{
    public partial class DocumentPages
    {
        public long DocumentId { get; set; }
        public long PhysicalFileId { get; set; }
        public int PageNumber { get; set; }

        public virtual Documents Document { get; set; }
        public virtual PhysicalFiles PhysicalFile { get; set; }
    }
}
