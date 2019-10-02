using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using System.IO;

namespace IdentityCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseKestrel()
                         .Build();

            host.Run();
        }
    }
}
