using System.Threading.Tasks;

namespace Identity.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> CheckUserPassword(string username, string password);

        Task<bool> ResetPassword(string username, string password);
    }
}
