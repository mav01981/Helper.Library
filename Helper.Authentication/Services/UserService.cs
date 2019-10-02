using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);
            
            if (EnumerableExtensions.Any(result.Errors))
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(DisplayErrorMessage)));

            return user;
        }

        public async Task<ApplicationUser> AddToRole(ApplicationUser user,string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (EnumerableExtensions.Any(result.Errors))
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(DisplayErrorMessage)));

            return user;
        }

        public async Task<ApplicationUser> AddClaims(ApplicationUser user, Claim claim)
        {
            var result = await _userManager.AddClaimAsync(user, claim);

            if (EnumerableExtensions.Any(result.Errors))
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(DisplayErrorMessage)));

            return user;
        }
    }
}
