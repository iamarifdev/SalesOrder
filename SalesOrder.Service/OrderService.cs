using AutoMapper;
using FluentValidation;
using SalesOrder.Common.DTO.Order;
using SalesOrder.Common.Exceptions;
using SalesOrder.Common.Models;
using SalesOrder.Common.Validators;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Service;

public interface IOrderService
{
    Task<PaginatedList<OrderDto>> GetOrders(PaginationQuery queryParams);
    Task<OrderDto> GetOrder(int id);
    Task<OrderDto> AddOrder(OrderCreateDto order);
    Task<OrderDto> UpdateOrder(int id, OrderUpdateDto order);
    Task<OrderDto> DeleteOrder(int id);
}

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IMapper mapper, IOrderRepository orderRepository)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
    }

    public async Task<PaginatedList<OrderDto>> GetOrders(PaginationQuery queryParams)
    {
        var orders = await _orderRepository.GetOrders(queryParams);
        return _mapper.Map<PaginatedList<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetOrder(int id)
    {
        var order = await _orderRepository.GetOrder(id);
        if (order == null) throw new OrderNotFoundException();
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> AddOrder(OrderCreateDto orderDto)
    {
        var validator = new OrderCreateDtoValidator();
        var result = await validator.ValidateAsync(orderDto);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.AddOrder(order);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> UpdateOrder(int id, OrderUpdateDto orderDto)
    {
        if (id != orderDto.Id) throw new InvalidOrderIdException();

        var validator = new OrderUpdateDtoValidator();
        var result = await validator.ValidateAsync(orderDto);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.UpdateOrder(order);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> DeleteOrder(int id)
    {
        var order = await _orderRepository.GetOrder(id);
        if (order == null) throw new OrderNotFoundException();

        await _orderRepository.DeleteOrder(id);
        return _mapper.Map<OrderDto>(order);
    }
}