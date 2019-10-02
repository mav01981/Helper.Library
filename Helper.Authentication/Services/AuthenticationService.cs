using Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Identity.Models;

namespace Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckUserPassword(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return false;

            return await this._userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> ResetPassword(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
