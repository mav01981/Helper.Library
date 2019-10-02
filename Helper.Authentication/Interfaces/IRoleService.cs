using System.Threading.Tasks;
using Identity.Models;

namespace Identity.Interfaces
{
    public interface IRoleService
    {
        Task<bool> Create(ApplicationRole role);
    }
}
