using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace API.Template.Utillities
{
    public static class GetAllRoutes
    {
        public static IApplicationBuilder AllRoutes(this IRouteBuilder routeBuilder, PathString pathString)
        {
            if (routeBuilder == null)
                throw new ArgumentNullException(nameof(routeBuilder));

            var app = routeBuilder.ApplicationBuilder;
            app.Map(pathString, builder => {
                builder.UseMiddleware<GetRoutes>(routeBuilder);
            });
            return app;
        }
    }
}
