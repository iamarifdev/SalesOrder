using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Data.Tests.Repositories;

[TestFixture]
public class OrderRepositoryTests
{
    [SetUp]
    public async Task SetUpAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _repository = new OrderRepository(_context);

        // Seed the database with test data
        var orders = new List<Order>
        {
            new() { Name = "Test 1", State = "State1" }
        };
        await _context.AddRangeAsync(orders);
        await _context.SaveChangesAsync();
    }

    [TearDown]
    public async Task CleanupAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    private ApplicationDbContext _context;
    private IOrderRepository _repository;

    [Test]
    public async Task GetOrders_ShouldReturnPaginatedList()
    {
        // Arrange
        var order1 = new Order { Name = "Order1", State = "State10" };
        var order2 = new Order { Name = "Order2", State = "State2" };
        var order3 = new Order { Name = "Order3", State = "State10" };
        await _context.Orders.AddRangeAsync(order1, order2, order3);
        await _context.SaveChangesAsync();

        var queryParams = new PaginationQuery
        {
            Page = 1,
            PageSize = 2,
            SearchTerm = "state10"
        };

        // Act
        var result = await _repository.GetOrders(queryParams);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result.Items.Should().Contain(order1).And.Contain(order3);
    }

    [Test]
    public async Task GetOrders_WithSearchTerm_ReturnsFilteredOrders()
    {
        // Arrange
        var paginationQuery = new PaginationQuery
        {
            SearchTerm = "Test",
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _repository.GetOrders(paginationQuery);

        // Assert
        result.Count.Should().Be(1);
        result.Items.Should().ContainSingle(o => o.Name == "Test 1");
    }

    [Test]
    public async Task GetOrders_WithoutSearchTerm_ReturnsAllOrders()
    {
        // Arrange
        var paginationQuery = new PaginationQuery
        {
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _repository.GetOrders(paginationQuery);

        // Assert
        result.Count.Should().Be(1);
        result.Items.Should().ContainSingle(o => o.Name == "Test 1");
    }

    [Test]
    public async Task GetOrder_WithValidId_ReturnsOrder()
    {
        // Arrange
        var orderId = 1;

        // Act
        var result = await _repository.GetOrder(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test 1");
        result.State.Should().Be("State1");
    }

    [Test]
    public async Task GetOrder_WithInvalidId_Should_Return_Null()
    {
        // Arrange
        var invalidOrderId = 999;

        // Act and Assert
        var result = await _repository.GetOrder(invalidOrderId);
        result.Should().BeNull();
    }

    [Test]
    public async Task GetOrder_ShouldReturnOrder()
    {
        // Arrange
        var order = new Order { Name = "Order1", State = "State1" };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOrder(order.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(order.Id);
        result.Name.Should().Be(order.Name);
    }

    [Test]
    public async Task AddOrder_ShouldAddOrder()
    {
        // Arrange
        var order = new Order { Name = "Order1", State = "State1" };

        // Act
        await _repository.AddOrder(order);

        // Assert
        _context.Orders.Should().Contain(order);
    }

    [Test]
    public async Task UpdateOrder_ShouldUpdateOrder()
    {
        // Arrange
        var order = new Order { Name = "Order1", State = "State1" };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        order.Name = "Updated Order";
        order.State = "Updated State";

        // Act
        await _repository.UpdateOrder(order);

        // Assert
        var updatedOrder = await _context.Orders.FindAsync(order.Id);
        updatedOrder.Should().NotBeNull();
        updatedOrder.Name.Should().Be(order.Name);
        updatedOrder.State.Should().Be(order.State);
        updatedOrder.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task DeleteOrder_WithValidOrderId_RemovesOrderFromDatabase()
    {
        var order = new Order { Name = "Test Order", State = "New York" };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteOrder(order.Id);

        // Assert
        var result = await _context.Orders.FindAsync(order.Id);
        result.Should().BeNull();
    }
}