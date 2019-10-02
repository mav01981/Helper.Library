using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Models;

namespace Identity.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> CreateUser(ApplicationUser user);
        Task<ApplicationUser> AddToRole(ApplicationUser user, string role);
        Task<ApplicationUser> AddClaims(ApplicationUser user, Claim claim);
    }
}
