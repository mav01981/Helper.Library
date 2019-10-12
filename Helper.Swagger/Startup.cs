using API.Template.Utillities;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using System;

public static class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ToDo API",
                Description = "A simple example ASP.NET Core Web API",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Jonathan Smart",
                    Email = string.Empty,
                    Url = new Uri("https://twitter.com/spboyer"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://example.com/license"),
                }
            });
        });

        services.AddMvcCore().AddApiExplorer();

        return services;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static IApplicationBuilder Configure(this IApplicationBuilder app, IHostingEnvironment env)
    {

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = string.Empty;
        });

        app.UseMvc(routes =>
        {
            routes.AllRoutes("/allroutes");

            routes.MapRoute(
                name: "default",
                template: "{*url}",
                defaults: new { controller = "Home", action = "Index" });
        });

        return app;
    }
}

