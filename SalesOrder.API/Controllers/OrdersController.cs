using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SalesOrder.API.Helpers;
using SalesOrder.Common.DTO.Order;
using SalesOrder.Common.Models;
using SalesOrder.Service;

namespace SalesOrder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiPaginatedResponse<OrderDto>>> GetOrders([FromQuery] PaginationQuery query)
    {
        try
        {
            var orders = await _orderService.GetOrders(query);
            return orders.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrder(int id)
    {
        try
        {
            var order = await _orderService.GetOrder(id);
            return order.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> AddOrder(OrderCreateDto dto)
    {
        try
        {
            var order = await _orderService.AddOrder(dto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order.ToApiResponse());
        }
        catch (ValidationException validationException)
        {
            return BadRequest(validationException.ToValidationErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> PutOrder(int id, OrderUpdateDto dto)
    {
        try
        {
            var order = await _orderService.UpdateOrder(id, dto);
            return order.ToApiResponse();
        }
        catch (ValidationException validationException)
        {
            return BadRequest(validationException.ToValidationErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> DeleteOrder(int id)
    {
        try
        {
            var order = await _orderService.DeleteOrder(id);
            return order.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }
}