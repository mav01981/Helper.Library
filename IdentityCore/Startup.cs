using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Identity;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityCore
{
    public class Startup : IStartup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            var factory = new IdentityContextFactory(); ;

            using (var db = factory.Create(DataSource.Sqllite,"Filename = MyDatabase.db"))
            {
                db.Database.EnsureCreated();
                
                db.Database.Migrate();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMembership("Filename = MyDatabase.db");
            services.AddLogging();
            services.AddSingleton<IUserService, UserService>();
            services.BuildServiceProvider();
            ;
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            var userService = app.ApplicationServices.GetService<IUserService>();

           var user =  userService.CreateUser(new ApplicationUser()
            {
                UserName = "Jsmart1",
                Email = "jsmart@trakGlobal.co.uk",
                PasswordHash = "password",
            });



            app.UseAuthentication();

        }
    }
}
