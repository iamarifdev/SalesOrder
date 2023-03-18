using SalesOrder.Data;
using SalesOrder.Data.Models;

namespace SalesOrder.API.Tests;

public static class TestDataHelper
{
    public static async Task SeedTestData(this ApplicationDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var order = new Order
        {
            Id = 1,
            Name = "Test Order",
            State = "Created",
            Windows = new List<Window>
            {
                new()
                {
                    Id = 1,
                    Name = "Window 1",
                    QuantityOfWindows = 4,
                    TotalSubElements = 2,
                    SubElements = new List<SubElement>
                    {
                        new()
                        {
                            Id = 1,
                            OrderId = 1,
                            Element = 1,
                            Type = "Frame",
                            Width = 100,
                            Height = 100
                        },
                        new()
                        {
                            Id = 2,
                            OrderId = 1,
                            Element = 2,
                            Type = "Glass",
                            Width = 100,
                            Height = 100
                        }
                    }
                }
            }
        };

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
    }
}