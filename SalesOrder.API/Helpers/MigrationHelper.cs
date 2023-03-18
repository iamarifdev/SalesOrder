using Microsoft.EntityFrameworkCore;
using SalesOrder.Data;

namespace SalesOrder.API.Helpers;

public static class MigrationHelper
{
    public static void UpdateDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        context?.Database.Migrate();
    }
}