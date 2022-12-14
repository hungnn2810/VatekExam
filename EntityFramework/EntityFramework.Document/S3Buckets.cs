using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EntityFramework.Document
{
    public partial class S3Buckets
    {
        public S3Buckets()
        {
            PhysicalFiles = new HashSet<PhysicalFiles>();
        }

        public short S3BucketId { get; set; }
        public string S3BucketName { get; set; }

        public virtual ICollection<PhysicalFiles> PhysicalFiles { get; set; }
    }
}
