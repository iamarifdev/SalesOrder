using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SalesOrder.API.Controllers;
using SalesOrder.Common.DTO.Order;
using SalesOrder.Common.Helpers;
using SalesOrder.Common.Models;
using SalesOrder.Data;

namespace SalesOrder.API.Tests.Controllers
{
    public class OrdersControllerTests : IDisposable
    {
        private WebApplicationFactory<OrdersController> _factory;
        private ApplicationDbContext _context;

        [SetUp]
        public async Task SetupAsync()
        {
            _factory = CustomWebApplicationFactory<OrdersController>.Create();
            
            // Initialize test data
            using var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await _context.SeedTestData();
        }

        [Test]
        public async Task GetOrders_ReturnsPaginatedOrders()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetJsonAsync("/api/Orders");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiPaginatedResponse<OrderDto>>();
            var ordersResponse = result.Result;
            
            ordersResponse.Items.Should().NotBeNull();
            ordersResponse.Count.Should().BeGreaterOrEqualTo(0);
        }

        [Test]
        public async Task GetOrder_WithValidId_ReturnsOrder()
        {
            // Arrange
            var client = _factory.CreateClient();
            const int orderId = 1;

            // Act
            var response = await client.GetAsync($"/api/Orders/{orderId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
            result.Result.Id.Should().Be(orderId);
        }

        [Test]
        public async Task AddOrder_WithValidData_ReturnsCreatedOrder()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newOrder = new OrderCreateDto
            {
                Name = "Test Order",
                State = "Test State",
            };

            // Act
            var response = await client.PostJsonAsync("/api/orders", newOrder);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
            result.Result.Should().NotBeNull();
            result.Result.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task PutOrder_WithValidData_ReturnsUpdatedOrder()
        {
            // Arrange
            var client = _factory.CreateClient();
            const int orderId = 1;
            var updatedOrder = new OrderUpdateDto
            {
                Id = orderId,
                Name = "Updated Order",
                State = "Updated State",
            };

            // Act
            var response = await client.PutJsonAsync($"/api/Orders/{orderId}", updatedOrder);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
            result.Result.Should().NotBeNull();

            result.Result.Id.Should().Be(orderId);
            result.Result.Name.Should().Be(updatedOrder.Name);
            result.Result.State.Should().Be(updatedOrder.State);
        }

        [Test]
        public async Task DeleteOrder_WithValidId_ReturnsDeletedOrder()
        {
            // Arrange
            var client = _factory.CreateClient();
            const int orderId = 1;

            // Act
            var response = await client.DeleteAsync($"/api/Orders/{orderId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
            result.Result.Should().NotBeNull();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            _context?.Dispose();
        }
    }
}