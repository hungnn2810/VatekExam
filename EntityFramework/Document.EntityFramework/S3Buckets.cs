using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Document.EntityFramework
{
    public partial class S3Buckets
    {
        public S3Buckets()
        {
            PhysialFiles = new HashSet<PhysialFiles>();
        }

        public short S3BucketId { get; set; }
        public string S3BucketName { get; set; }

        public virtual ICollection<PhysialFiles> PhysialFiles { get; set; }
    }
}
