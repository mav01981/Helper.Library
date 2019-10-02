using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Helper.JWT
{
    public static class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="issuer"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IServiceCollection AddJWT(this IServiceCollection services,
            string issuer, string key)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = issuer,
                      ValidAudience = issuer,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                  };
              });

            return services;
        }
    }
}
