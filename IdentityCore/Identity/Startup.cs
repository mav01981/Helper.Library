using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Identity
{
    public static class Startup
    {
        public static IServiceCollection AddMembership(this IServiceCollection services, string sqlConnection)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlite("Filename=TestDatabase.db"))
                 .AddIdentity<ApplicationUser, ApplicationRole>()
                     .AddEntityFrameworkStores<IdentityContext>()
                     .AddDefaultTokenProviders()
                        .AddPasswordValidator<UsernameAsPasswordValidator<ApplicationUser>>();

            return services;
        }
    }

    public class UsernameAsPasswordValidator<TUser> : IPasswordValidator<TUser>
        where TUser : ApplicationUser
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "You cannot use your username as your password"
                }));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
