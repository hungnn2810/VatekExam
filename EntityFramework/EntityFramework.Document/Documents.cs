using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EntityFramework.Document
{
    public partial class Documents
    {
        public Documents()
        {
            Bookmarks = new HashSet<Bookmarks>();
            DocumentPages = new HashSet<DocumentPages>();
        }

        public long DocumentId { get; set; }
        public long CategoryId { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public bool? Visible { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<Bookmarks> Bookmarks { get; set; }
        public virtual ICollection<DocumentPages> DocumentPages { get; set; }
    }
}
