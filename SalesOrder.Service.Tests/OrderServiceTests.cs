using AutoMapper;
using FluentAssertions;
using Moq;
using SalesOrder.Common.DTO.Order;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;
using SalesOrder.Common.Exceptions;

namespace SalesOrder.Service.Tests;

[TestFixture]
public class OrderServiceTests
{
    private IOrderService _orderService;
    private Mock<IMapper> _mapperMock;
    private Mock<IOrderRepository> _orderRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mapperMock.Object, _orderRepositoryMock.Object);
    }

    [Test]
    public async Task GetOrders_ShouldReturnPaginatedList()
    {
        // Arrange
        var queryParams = new PaginationQuery { Page = 1, PageSize = 10 };
        var orders = new List<Order> { new() { Id = 1, Name = "Building 1", State = "NY" } };
        var paginatedOrders = new PaginatedList<Order> { Items = orders, Count = 1 };
        _orderRepositoryMock.Setup(x => x.GetOrders(queryParams)).ReturnsAsync(paginatedOrders);
        _mapperMock.Setup(x => x.Map<PaginatedList<OrderDto>>(paginatedOrders)).Returns(
            new PaginatedList<OrderDto>
            {
                Items = orders.Select(o => new OrderDto { Id = o.Id, Name = o.Name, State = o.State }).ToList(),
                Count = 1
            });

        // Act
        var result = await _orderService.GetOrders(queryParams);

        // Assert
        result.Should().NotBeNull();
        result.Items.Count().Should().Be(1);
    }

    [Test]
    public async Task GetOrder_ExistingId_ShouldReturnOrderDto()
    {
        // Arrange
        const int orderId = 1;
        var order = new Order { Id = orderId, Name = "Building 1", State = "NY" };
        _orderRepositoryMock.Setup(x => x.GetOrder(orderId)).ReturnsAsync(order);
        _mapperMock.Setup(x => x.Map<OrderDto>(order))
            .Returns(new OrderDto { Id = orderId, Name = "Building 1", State = "NY" });

        // Act
        var result = await _orderService.GetOrder(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
        result.Name.Should().Be(order.Name);
    }

    [Test]
    public void GetOrder_NonExistingId_ShouldThrowOrderNotFoundException()
    {
        // Arrange
        const int orderId = 1;
        _orderRepositoryMock.Setup(x => x.GetOrder(orderId)).ReturnsAsync((Order)null);

        // Act
        Func<Task> act = async () => await _orderService.GetOrder(orderId);

        // Assert
        act.Should().ThrowAsync<OrderNotFoundException>();
    }

    [Test]
    public async Task AddOrder_ValidOrderCreateDto_ShouldReturnOrderDto()
    {
        // Arrange
        var orderCreateDto = new OrderCreateDto { Name = "Building 1", State = "NY" };
        var order = new Order { Id = 1, Name = orderCreateDto.Name, State = orderCreateDto.State };
        _mapperMock.Setup(x => x.Map<Order>(orderCreateDto)).Returns(order);
        _orderRepositoryMock.Setup(x => x.AddOrder(order)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<OrderDto>(order))
            .Returns(new OrderDto { Id = order.Id, Name = order.Name, State = order.State });

        // Act
        var result = await _orderService.AddOrder(orderCreateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(order.Id);
        result.Name.Should().Be(order.Name);
        result.State.Should().Be(order.State);
    }

    [Test]
    public async Task UpdateOrder_ValidOrderUpdateDto_ShouldReturnOrderDto()
    {
        // Arrange
        const int orderId = 1;
        var orderUpdateDto = new OrderUpdateDto { Id = orderId, Name = "Building 1", State = "NY" };
        var order = new Order { Id = orderId, Name = orderUpdateDto.Name, State = orderUpdateDto.State };
        _mapperMock.Setup(x => x.Map<Order>(orderUpdateDto)).Returns(order);
        _orderRepositoryMock.Setup(x => x.UpdateOrder(order)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<OrderDto>(order))
            .Returns(new OrderDto { Id = order.Id, Name = order.Name, State = order.State });

        // Act
        var result = await _orderService.UpdateOrder(orderId, orderUpdateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(order.Id);
        result.Name.Should().Be(order.Name);
    }

    [Test]
    public void UpdateOrder_MismatchedId_ShouldThrowInvalidOrderIdException()
    {
        // Arrange
        const int id = 1;
        var orderUpdateDto = new OrderUpdateDto { Id = 2, Name = "Building 1", State = "NY" };

        // Act
        Func<Task> act = async () => await _orderService.UpdateOrder(id, orderUpdateDto);

        // Assert
        act.Should().ThrowAsync<InvalidOrderIdException>();
    }

    [Test]
    public async Task DeleteOrder_ExistingOrder_ShouldReturnDeletedOrderDto()
    {
        // Arrange
        const int id = 1;
        var order = new Order { Id = id, Name = "Building 1", State = "NY" };
        var orderDto = new OrderDto { Id = id, Name = "Building 1", State = "NY" };

        _orderRepositoryMock.Setup(x => x.GetOrder(id)).ReturnsAsync(order);
        _orderRepositoryMock.Setup(x => x.DeleteOrder(id)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<OrderDto>(order)).Returns(orderDto);

        // Act
        var result = await _orderService.DeleteOrder(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be(order.Name);
    }

    [Test]
    public void DeleteOrder_NonExistingOrder_ShouldThrowOrderNotFoundException()
    {
        // Arrange
        const int id = 1;

        _orderRepositoryMock.Setup(x => x.GetOrder(id)).ReturnsAsync((Order)null);

        // Act
        Func<Task> act = async () => await _orderService.DeleteOrder(id);

        // Assert
        act.Should().ThrowAsync<OrderNotFoundException>();
    }
}