using Microsoft.EntityFrameworkCore;
using SalesOrder.Common.Exceptions;
using SalesOrder.Common.Helpers;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;

namespace SalesOrder.Data.Repositories;

public interface IOrderRepository
{
    Task<PaginatedList<Order>> GetOrders(PaginationQuery queryParams);
    Task<Order> GetOrder(int id);
    Task AddOrder(Order order);
    Task UpdateOrder(Order order);
    Task DeleteOrder(int id);
}

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Order>> GetOrders(PaginationQuery queryParams)
    {
        var query = _context.Orders.AsQueryable();

        if (queryParams.SearchTerm.IsNotEmpty())
        {
            var searchTerm = queryParams.SearchTerm.ToLower();
            query = query.Where(o =>
                o.Name.ToLower().Contains(searchTerm) ||
                o.State.ToLower().Contains(searchTerm)
            );
        }

        var count = await query.CountAsync();

        var list = await query
            .OrderByField(queryParams.SortField, queryParams.SortOrder)
            .Skip(queryParams.PageSize * (queryParams.Page - 1))
            .Take(queryParams.PageSize)
            .Include(o => o.Windows).ThenInclude(w => w.SubElements)
            .ToListAsync();

        return new PaginatedList<Order> { Count = count, Items = list };
    }

    public async Task<Order> GetOrder(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task AddOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrder(Order order)
    {
        order.UpdatedAt = DateTime.Now;
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) throw new OrderNotFoundException();
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}