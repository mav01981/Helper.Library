using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System;

public interface IStartup
{
    IServiceProvider ConfigureServices(IServiceCollection services);
    void Configure(IApplicationBuilder app);
}