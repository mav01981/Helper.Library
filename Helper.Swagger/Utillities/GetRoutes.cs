using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Template.Utillities
{
    public class GetRoutes
    {
        private IRouteBuilder _routeBuilder;

        public GetRoutes(RequestDelegate next, IRouteBuilder routeBuilder)
        {
            _routeBuilder = routeBuilder;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var globals = _routeBuilder?.Routes?.Where(r => r.GetType() != typeof(Route)).Select(r =>
            {
                Route _r = ((Route)(r));
                return new
                {
                    _r.Name,
                    Template = _r.RouteTemplate,
                    DefaultAction = _r.Defaults["action"],
                    DefaultController = _r.Defaults["controller"],
                };
            });

            IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider = context.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();

            var actions = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select(a => new
            {
                Action = a.RouteValues["action"],
                Controller = a.RouteValues["controller"],
                Name = a?.AttributeRouteInfo?.Name,
                Templates = new string[] { a?.AttributeRouteInfo?.Template },
                HttpMethods = a?.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods
            });

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { globals, actions })).ConfigureAwait(false);
            return;
        }
    }
}
