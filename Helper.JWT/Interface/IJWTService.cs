using Helper.JWT.Model;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Helper.JWT
{
    public interface IJWTService
    {
        Task<string> GenerateJWTToken(JWTInput jWTInput, IEnumerable<Claim> claims);
    }
}
