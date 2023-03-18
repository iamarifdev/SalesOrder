using SalesOrder.Common.DTO.Order;
using SalesOrder.Common.Models;

namespace SalesOrder.App.Services;

public interface IOrderService
{
    Task<PaginatedList<OrderDto>> GetAllOrdersAsync(PaginationQuery paginationQuery);
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    Task<OrderDto> AddOrderAsync(OrderCreateDto dto);
    Task<OrderDto> UpdateOrderAsync(int orderId, OrderUpdateDto dto);
    Task<OrderDto> DeleteOrderAsync(int orderId);
}

public class OrderService : BaseService<OrderDto, OrderCreateDto, OrderUpdateDto>, IOrderService
{
    public OrderService(HttpClient httpClient) : base(httpClient)
    {
    }

    protected override string EndpointUrl => "Orders";

    public async Task<PaginatedList<OrderDto>> GetAllOrdersAsync(PaginationQuery paginationQuery)
    {
        return await GetAllAsync(paginationQuery);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int orderId)
    {
        return await GetByIdAsync(orderId);
    }

    public async Task<OrderDto> AddOrderAsync(OrderCreateDto dto)
    {
        return await AddAsync(dto);
    }

    public async Task<OrderDto> UpdateOrderAsync(int orderId, OrderUpdateDto dto)
    {
        return await UpdateAsync(orderId, dto);
    }

    public async Task<OrderDto> DeleteOrderAsync(int orderId)
    {
        return await DeleteAsync(orderId);
    }
}