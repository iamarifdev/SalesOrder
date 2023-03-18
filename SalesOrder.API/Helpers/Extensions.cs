using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SalesOrder.API.AutoMappingProfiles;
using SalesOrder.Data.Repositories;
using SalesOrder.Service;

namespace SalesOrder.API.Helpers;

public static class Extensions
{
    public static IServiceCollection RegisterAutoMappingProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddProfile(new ModelMappingProfile()), typeof(Program));
        return services;
    }

    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IWindowService, WindowService>();
        services.AddScoped<IWindowRepository, WindowRepository>();

        services.AddScoped<ISubElementService, SubElementService>();
        services.AddScoped<ISubElementRepository, SubElementRepository>();

        return services;
    }

    public static bool IsRelational(this DbContextOptions options)
    {
        return options.FindExtension<RelationalOptionsExtension>() != null;
    }
}