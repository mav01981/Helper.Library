using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}