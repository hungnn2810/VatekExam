using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Document.EntityFramework
{
    public partial class PhysialFiles
    {
        public long PhysicalFileId { get; set; }
        public string PhysicalFileName { get; set; }
        public string PhysicalFileExtention { get; set; }
        public long FileLengthInBytes { get; set; }
        public short? S3BucketId { get; set; }
        public string S3FileKey { get; set; }
        public long DocumentId { get; set; }
        public int PageNumber { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Documents Document { get; set; }
        public virtual S3Buckets S3Bucket { get; set; }
    }
}
