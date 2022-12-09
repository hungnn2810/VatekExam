using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Identity.EntityFramework
{
    public partial class IdentityResources
    {
        public IdentityResources()
        {
            IdentityResourceClaims = new HashSet<IdentityResourceClaims>();
            IdentityResourceProperties = new HashSet<IdentityResourceProperties>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool NonEditable { get; set; }

        public virtual ICollection<IdentityResourceClaims> IdentityResourceClaims { get; set; }
        public virtual ICollection<IdentityResourceProperties> IdentityResourceProperties { get; set; }
    }
}
