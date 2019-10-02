using Identity.Interfaces;

using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;

namespace Identity
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> Create(ApplicationRole role)
        {
            var result = await this._roleManager.CreateAsync(role);

            if (result.Errors.Any())
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(DisplayErrorMessage)));

            return result.Succeeded;
        }
    }
}