using Helper.JWT.Model;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helper.JWT
{
    public class JWTService : IJWTService
    {
        /// <summary>
        /// Create Json Web Token
        /// </summary>
        /// <param name="jWTInput"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateJWTToken(JWTInput jWTInput, IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTInput.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(jWTInput.Issuer,
              jWTInput.Issuer,
              claims,
              expires: jWTInput.ExpiresOn,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public interface IJWTService
    {
        string GenerateJWTToken(JWTInput jWTInput, IEnumerable<Claim> claims);
    }
}
