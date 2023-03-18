using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SalesOrder.API.Controllers;
using SalesOrder.Common.DTO.Element;
using SalesOrder.Common.Helpers;
using SalesOrder.Common.Models;
using SalesOrder.Data;

namespace SalesOrder.API.Tests.Controllers;

public class SubElementsControllerTests : WebApplicationFactory<SubElementsController>
{
    private WebApplicationFactory<SubElementsController> _factory;
    private ApplicationDbContext _context;

    [SetUp]
    public async Task SetupAsync()
    {
        _factory = new WebApplicationFactory<SubElementsController>().WithWebHostBuilder(_ => { });

        // Initialize test data
        using var scope = _factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await _context.SeedTestData();
    }

    [Test]
    public async Task GetSubElements_ReturnsPaginatedSubElements()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetJsonAsync("/api/SubElements");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiPaginatedResponse<SubElementDto>>();
        var subElementsResponse = result.Result;

        subElementsResponse.Items.Should().NotBeNull();
        subElementsResponse.Count.Should().BeGreaterOrEqualTo(0);
    }

    [Test]
    public async Task GetSubElement_WithValidId_ReturnsSubElement()
    {
        // Arrange
        var client = _factory.CreateClient();
        const int subElementId = 1;

        // Act
        var response = await client.GetAsync($"/api/SubElements/{subElementId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<SubElementDto>>();
        result.Result.Id.Should().Be(subElementId);
    }
    
    [Test]
    public async Task AddSubElement_WithValidData_ReturnsCreatedSubElement()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newSubElement = new SubElementCreateDto
        {
            OrderId = 1,
            WindowId = 1,
            Element = 1,
            Type = "Test Type",
            Width = 100,
            Height = 100,
        };

        // Act
        var response = await client.PostJsonAsync("/api/SubElements", newSubElement);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<SubElementDto>>();
        result.Result.Should().NotBeNull();
        result.Result.Id.Should().BeGreaterThan(0);
    }
    
    [Test]
    public async Task PutSubElement_WithValidData_ReturnsUpdatedSubElement()
    {
        // Arrange
        var client = _factory.CreateClient();
        const int subElementId = 1;
        var updatedSubElement = new SubElementUpdateDto
        {
            Id = subElementId,
            OrderId = 1,
            WindowId = 1,
            Element = 1001,
            Type = "Updated Type",
            Width = 100,
            Height = 200
        };

        // Act
        var response = await client.PutJsonAsync($"/api/SubElements/{subElementId}", updatedSubElement);

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<SubElementDto>>();
        result.Result.Should().NotBeNull();

        result.Result.Id.Should().Be(subElementId);
        result.Result.Element.Should().Be(updatedSubElement.Element);
        result.Result.Type.Should().Be(updatedSubElement.Type);
        result.Result.Width.Should().Be(updatedSubElement.Width);
        result.Result.Height.Should().Be(updatedSubElement.Height);
    }

    [Test]
    public async Task DeleteSubElement_WithValidId_ReturnsDeletedSubElement()
    {
        // Arrange
        var client = _factory.CreateClient();
        const int subElementId = 1;

        // Act
        var response = await client.DeleteAsync($"/api/SubElements/{subElementId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<SubElementDto>>();
        result.Result.Should().NotBeNull();
    }
}