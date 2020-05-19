using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Helper.Swagger
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, SwaggerOptions swaggerOptions)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = swaggerOptions.Version,
                    Title = swaggerOptions.Title,
                    Description = swaggerOptions.Description,
                    TermsOfService = new Uri(swaggerOptions.TermsOfServiceUrl),
                    Contact = new OpenApiContact
                    {
                        Name = swaggerOptions.ContactName,
                        Email = swaggerOptions.ContactEmail,
                        Url = new Uri(swaggerOptions.ContactUrl),
                    },
                    License = new OpenApiLicense
                    {
                        Name = swaggerOptions.LicenseName,
                        Url = new Uri(swaggerOptions.LicenseUrl),
                    }
                });
            });

            services.AddMvcCore().AddApiExplorer();

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, string stylePath)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                c.InjectStylesheet(stylePath);
            });

            return app;
        }
    }
}

