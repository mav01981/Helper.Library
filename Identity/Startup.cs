using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Identity
{
    public static class Startup
    {
        public static IServiceCollection AddMembership(this IServiceCollection services, string sqlConnection)
        {
            services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlite(sqlConnection))
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();

            return services;
        }
    }
}
