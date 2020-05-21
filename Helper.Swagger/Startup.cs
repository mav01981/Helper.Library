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
                    TermsOfService = swaggerOptions.TermsOfServiceUrl != null ? new Uri(swaggerOptions.TermsOfServiceUrl) : null,
                    Contact = new OpenApiContact
                    {
                        Name = swaggerOptions.ContactName,
                        Email = swaggerOptions.ContactEmail,
                        Url = swaggerOptions.ContactUrl != null ? new Uri(swaggerOptions.ContactUrl):null,
                    },
                    License = new OpenApiLicense
                    {
                        Name = swaggerOptions.LicenseName,
                        Url = swaggerOptions.LicenseUrl != null ? new Uri(swaggerOptions.LicenseUrl) : null,
                    }
                });
            });

            services.AddMvcCore().AddApiExplorer();

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, SwaggerUISettings settings)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(settings.EndPointUrl, settings.EndPointName);
                c.RoutePrefix = string.Empty;
                c.InjectStylesheet(settings.StylePath);
            });

            return app;
        }
    }
}

