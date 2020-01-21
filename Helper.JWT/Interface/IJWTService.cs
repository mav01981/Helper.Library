using Helper.JWT.Model;
using System.Collections.Generic;
using System.Security.Claims;

namespace Helper.JWT
{
    public interface IJWTService
    {
        string GenerateJWTToken(JWTInput jWTInput, IEnumerable<Claim> claims);
    }
}
