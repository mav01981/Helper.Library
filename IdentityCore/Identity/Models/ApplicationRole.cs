using System;
using System.Collections.Generic;
using Identity.Models;

using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}