using AutoMapper;
using FluentAssertions;
using Moq;
using SalesOrder.Common.DTO.Window;
using SalesOrder.Common.Exceptions;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Service.Tests;

[TestFixture]
public class WindowServiceTests
{
    private IWindowService _windowService;
    private Mock<IMapper> _mapperMock;
    private Mock<IWindowRepository> _windowRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>();
        _windowRepositoryMock = new Mock<IWindowRepository>();
        _windowService = new WindowService(_mapperMock.Object, _windowRepositoryMock.Object);
    }

    [Test]
    public async Task GetWindows_ShouldReturnPaginatedList()
    {
        // Arrange
        var queryParams = new WindowPaginationQuery { Page = 1, PageSize = 10 };
        var windows = new List<Window>
        {
            new()
            {
                Id = 1, OrderId = 1, Name = "Window 1", QuantityOfWindows = 5, TotalSubElements = 10
            }
        };
        var paginatedWindows = new PaginatedList<Window> { Items = windows, Count = 1 };
        _windowRepositoryMock.Setup(x => x.GetWindows(queryParams)).ReturnsAsync(paginatedWindows);
        _mapperMock.Setup(x => x.Map<PaginatedList<WindowDto>>(paginatedWindows)).Returns(
            new PaginatedList<WindowDto>
            {
                Items = windows.Select(w => new WindowDto
                    { Id = 1, OrderId = 1, Name = "Window 1", QuantityOfWindows = 5, TotalSubElements = 10 }).ToList(),
                Count = 1
            });

        // Act
        var result = await _windowService.GetWindows(queryParams);

        // Assert
        result.Should().NotBeNull();
        result.Items.Count().Should().Be(1);
    }

    [Test]
    public async Task GetWindow_ExistingWindow_ShouldReturnWindowDto()
    {
        // Arrange
        const int id = 1;
        var window = new Window
            { Id = 1, OrderId = 1, Name = "Window 1", QuantityOfWindows = 5, TotalSubElements = 10 };
        var windowDto = new WindowDto
            { Id = 1, OrderId = 1, Name = "Window 1", QuantityOfWindows = 5, TotalSubElements = 10 };

        _windowRepositoryMock.Setup(x => x.GetWindow(id)).ReturnsAsync(window);
        _mapperMock.Setup(x => x.Map<WindowDto>(window)).Returns(windowDto);

        // Act
        var result = await _windowService.GetWindow(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be(window.Name);
        result.QuantityOfWindows.Should().Be(window.QuantityOfWindows);
    }

    [Test]
    public void GetWindow_NonExistingWindow_ShouldThrowWindowNotFoundException()
    {
        // Arrange
        const int id = 1;

        _windowRepositoryMock.Setup(x => x.GetWindow(id)).ReturnsAsync((Window)null);

        // Act
        Func<Task> act = async () => await _windowService.GetWindow(id);

        // Assert
        act.Should().ThrowAsync<WindowNotFoundException>();
    }

    [Test]
    public async Task AddWindow_ValidWindowCreateDto_ShouldReturnWindowDto()
    {
        // Arrange
        var windowCreateDto = new WindowCreateDto { Name = "New Window", QuantityOfWindows = 5, OrderId = 1 };
        var window = new Window
            { Id = 1, Name = "New Window", QuantityOfWindows = 5, OrderId = 1, TotalSubElements = 0 };
        var windowDto = new WindowDto
            { Id = 1, Name = "New Window", QuantityOfWindows = 5, OrderId = 1, TotalSubElements = 0 };

        _mapperMock.Setup(x => x.Map<Window>(windowCreateDto)).Returns(window);
        _windowRepositoryMock.Setup(x => x.AddWindow(window)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<WindowDto>(window)).Returns(windowDto);

        // Act
        var result = await _windowService.AddWindow(windowCreateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(window.Id);
        result.Name.Should().Be(window.Name);
        result.QuantityOfWindows.Should().Be(window.QuantityOfWindows);
        result.TotalSubElements.Should().Be(window.TotalSubElements);
    }

    [Test]
    public void UpdateWindow_MismatchedId_ShouldThrowInvalidWindowIdException()
    {
        // Arrange
        const int id = 1;
        var windowUpdateDto = new WindowUpdateDto { Id = 2, Name = "Updated Window", QuantityOfWindows = 5 };

        // Act
        Func<Task> act = async () => await _windowService.UpdateWindow(id, windowUpdateDto);

        // Assert
        act.Should().ThrowAsync<InvalidWindowIdException>();
    }

    [Test]
    public async Task UpdateWindow_ValidWindowUpdateDto_ShouldReturnUpdatedWindowDto()
    {
        // Arrange
        const int id = 1;
        var windowUpdateDto = new WindowUpdateDto
            { Id = id, OrderId = 1, Name = "Updated Window", QuantityOfWindows = 10 };
        var window = new Window { Id = id, OrderId = 1, Name = "Initial Window", QuantityOfWindows = 5 };

        _windowRepositoryMock.Setup(x => x.UpdateWindow(It.IsAny<Window>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<Window>(windowUpdateDto)).Returns(window);
        _mapperMock.Setup(x => x.Map<WindowDto>(window)).Returns(new WindowDto
            { Id = id, OrderId = 1, Name = "Updated Window", QuantityOfWindows = 10 });

        // Act
        var result = await _windowService.UpdateWindow(id, windowUpdateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be("Updated Window");
        result.QuantityOfWindows.Should().Be(10);
        result.TotalSubElements.Should().Be(0);
    }
    
    [Test]
    public async Task DeleteWindow_ExistingWindow_ShouldReturnDeletedWindowDto()
    {
        // Arrange
        const int id = 1;
        var window = new Window { Id = id, OrderId = 1, Name = "Window 1", QuantityOfWindows = 5 };
        var windowDto = new WindowDto { Id = id, OrderId = 1, Name = "Window 1", QuantityOfWindows = 5 };

        _windowRepositoryMock.Setup(x => x.GetWindow(id)).ReturnsAsync(window);
        _windowRepositoryMock.Setup(x => x.DeleteWindow(id)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<WindowDto>(window)).Returns(windowDto);

        // Act
        var result = await _windowService.DeleteWindow(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be(window.Name);
        result.QuantityOfWindows.Should().Be(window.QuantityOfWindows);
    }

    [Test]
    public void DeleteWindow_NonExistingWindow_ShouldThrowWindowNotFoundException()
    {
        // Arrange
        const int id = 1;

        _windowRepositoryMock.Setup(x => x.GetWindow(id)).ReturnsAsync((Window)null);

        // Act
        Func<Task> act = async () => await _windowService.DeleteWindow(id);

        // Assert
        act.Should().ThrowAsync<WindowNotFoundException>();
    }
}