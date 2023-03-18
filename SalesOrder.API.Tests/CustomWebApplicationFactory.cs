using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace SalesOrder.API.Tests;

using Microsoft.AspNetCore.Mvc.Testing;

public static class CustomWebApplicationFactory<T> where T: class
{
    public static WebApplicationFactory<T> Create()
    {
        return new WebApplicationFactory<T>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                context.HostingEnvironment.EnvironmentName = "Test";
            });
        });
    }
}