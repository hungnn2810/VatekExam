using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Document.EntityFramework
{
    public partial class Documents
    {
        public Documents()
        {
            BookMarks = new HashSet<BookMarks>();
        }

        public long DocumentId { get; set; }
        public long CategoryId { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public long PhysicalFileId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Categories Category { get; set; }
        public virtual PhysicalFiles PhysicalFile { get; set; }
        public virtual ICollection<BookMarks> BookMarks { get; set; }
    }
}
