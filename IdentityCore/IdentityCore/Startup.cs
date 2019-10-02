using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

using Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityCore
{
    public class Startup : IStartup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            var userStore = new UserStore();

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
            services.AddMembership("");

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            app.UseAuthentication();

        }
    }
}
