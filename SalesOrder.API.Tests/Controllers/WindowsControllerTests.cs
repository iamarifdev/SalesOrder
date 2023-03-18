using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SalesOrder.API.Controllers;
using SalesOrder.Common.DTO.Window;
using SalesOrder.Common.Helpers;
using SalesOrder.Common.Models;
using SalesOrder.Data;

namespace SalesOrder.API.Tests.Controllers
{
    public class WindowsControllerTests : IDisposable
    {
        private WebApplicationFactory<WindowsController> _factory;
        private ApplicationDbContext _context;

        [SetUp]
        public async Task SetupAsync()
        {
            _factory = CustomWebApplicationFactory<WindowsController>.Create();

            // Initialize test data
            using var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await _context.SeedTestData();
        }

        [Test]
        public async Task GetWindows_ReturnsPaginatedWindows()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetJsonAsync("/api/Windows");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiPaginatedResponse<WindowDto>>();
            var windowsResponse = result.Result;

            windowsResponse.Items.Should().NotBeNull();
            windowsResponse.Count.Should().BeGreaterOrEqualTo(0);
        }

        [Test]
        public async Task GetWindow_WithValidId_ReturnsWindow()
        {
            // Arrange
            var client = _factory.CreateClient();
            const int windowId = 1;

            // Act
            var response = await client.GetAsync($"/api/Windows/{windowId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<WindowDto>>();
            result.Result.Id.Should().Be(windowId);
        }

        [Test]
        public async Task AddWindow_WithValidData_ReturnsCreatedWindow()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newWindow = new WindowCreateDto
            {
                OrderId = 1,
                Name = "Test Window",
                QuantityOfWindows = 2,
            };

            // Act
            var response = await client.PostJsonAsync("/api/Windows", newWindow);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<WindowDto>>();
            result.Result.Should().NotBeNull();
            result.Result.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task PutWindow_WithValidData_ReturnsUpdatedWindow()
        {
            // Arrange
            var client = _factory.CreateClient();
            const int windowId = 1;
            var updatedWindow = new WindowUpdateDto
            {
                Id = windowId,
                Name = "Updated Window",
                QuantityOfWindows = 5,
                OrderId = 1,
            };

            // Act
            var response = await client.PutJsonAsync($"/api/Windows/{windowId}", updatedWindow);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<WindowDto>>();
            result.Result.Should().NotBeNull();

            result.Result.Id.Should().Be(windowId);
            result.Result.Name.Should().Be(updatedWindow.Name);
            result.Result.QuantityOfWindows.Should().Be(updatedWindow.QuantityOfWindows);
        }

        [Test]
        public async Task DeleteWindow_WithValidId_ReturnsDeletedWindow()
        {
            // Arrange
            var client = _factory.CreateClient();
            const int windowId = 1;

            // Act
            var response = await client.DeleteAsync($"/api/Windows/{windowId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<WindowDto>>();
            result.Result.Should().NotBeNull();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            _context?.Dispose();
        }
    }
}