using Microsoft.EntityFrameworkCore;
using SalesOrder.Data.Models;

namespace SalesOrder.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Window> Windows { get; set; }
    public DbSet<SubElement> SubElements { get; set; }
}