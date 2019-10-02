using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            var startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //configure console logging
            //serviceProvider
            //    .GetService<ILoggerFactory>()
            //    .AddConsole(LogLevel.Debug);


            var userService = serviceProvider.GetService<IUserService>();
            var roleService = serviceProvider.GetService<IRoleService>();

            //await roleService.Create(new ApplicationRole()
            //{
            //   Id = new Guid(),
            //   Name = "Admin"
            //});

            var user = await userService.CreateUser(new ApplicationUser()
            {
                Email = "Jsmart@TrakGlobal.co.uk",
                UserName = "jPierce",
                PasswordHash = "Test"
            });

            await userService.AddToRole(user, "Admin");

            await userService.AddClaims(user, new Claim(ClaimTypes.Email, user.Email));

            Console.ReadLine();

        }
    }
}
