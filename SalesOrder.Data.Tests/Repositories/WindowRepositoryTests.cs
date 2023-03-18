using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Data.Tests.Repositories;

[TestFixture]
public class WindowRepositoryTests
{
    [SetUp]
    public async Task SetUpAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _windowRepository = new WindowRepository(_context);
    }

    [TearDown]
    public async Task CleanupAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    private ApplicationDbContext _context;
    private IWindowRepository _windowRepository;

    [Test]
    public async Task GetWindows_WithNoSearchParams_ReturnsAllWindows()
    {
        // Arrange
        var window1 = new Window { Name = "Window 1", OrderId = 1 };
        var window2 = new Window { Name = "Window 2", OrderId = 1 };
        var window3 = new Window { Name = "Window 3", OrderId = 2 };
        var windows = new List<Window> { window1, window2, window3 };

        await _context.Windows.AddRangeAsync(windows);
        await _context.SaveChangesAsync();

        var queryParams = new WindowPaginationQuery();

        // Act
        var result = await _windowRepository.GetWindows(queryParams);
        var items = result.Items.ToList();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(windows.Count));
            Assert.That(items[0].Name, Is.EqualTo(windows[0].Name));
            Assert.That(items[1].Name, Is.EqualTo(windows[1].Name));
            Assert.That(items[2].Name, Is.EqualTo(windows[2].Name));
        });
    }

    [Test]
    public async Task GetWindows_WithSearchTerm_ReturnsMatchingWindows()
    {
        // Arrange
        var window1 = new Window { Name = "Window 1", OrderId = 1 };
        var window2 = new Window { Name = "Window 2", OrderId = 1 };
        var window3 = new Window { Name = "Another Window", OrderId = 2 };
        var windows = new List<Window> { window1, window2, window3 };

        await _context.Windows.AddRangeAsync(windows);
        await _context.SaveChangesAsync();

        var queryParams = new WindowPaginationQuery { SearchTerm = "Window" };

        // Act
        var result = await _windowRepository.GetWindows(queryParams);
        var items = result.Items.ToList();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(items[0].Name, Is.EqualTo(window1.Name));
            Assert.That(items[1].Name, Is.EqualTo(window2.Name));
        });
    }

    [Test]
    public async Task GetWindows_WithOrderId_ReturnsWindowsForThatOrder()
    {
        // Arrange
        var window1 = new Window { Name = "Window 1", OrderId = 1 };
        var window2 = new Window { Name = "Window 2", OrderId = 1 };
        var window3 = new Window { Name = "Window 3", OrderId = 2 };
        var windows = new List<Window> { window1, window2, window3 };

        await _context.Windows.AddRangeAsync(windows);
        await _context.SaveChangesAsync();

        var queryParams = new WindowPaginationQuery { OrderId = 1 };

        // Act
        var result = await _windowRepository.GetWindows(queryParams);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Items.ElementAt(0).Name, Is.EqualTo(window1.Name));
            Assert.That(result.Items.ElementAt(1).Name, Is.EqualTo(window2.Name));
        });
    }

    [Test]
    public async Task GetWindows_WithPagination_ReturnsCorrectWindows()
    {
        // Arrange
        var data = new List<Window>
        {
            new() { Id = 1, Name = "Window 1", OrderId = 1 },
            new() { Id = 2, Name = "Window 2", OrderId = 1 },
            new() { Id = 3, Name = "Window 3", OrderId = 2 },
            new() { Id = 4, Name = "Window 4", OrderId = 2 }
        };

        await _context.Windows.AddRangeAsync(data);
        await _context.SaveChangesAsync();

        var paginationQuery = new WindowPaginationQuery
        {
            Page = 2,
            PageSize = 2,
            SortField = "Name",
            SortOrder = "asc"
        };

        // Act
        var result = await _windowRepository.GetWindows(paginationQuery);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(4);
        result.Items.Should().HaveCount(2);
        result.Items.First().Id.Should().Be(3);
        result.Items.Last().Id.Should().Be(4);
    }

    [Test]
    public async Task GetWindow_WithValidId_ReturnsCorrectWindow()
    {
        // Arrange
        var window = new Window { Name = "Window 1", OrderId = 1 };
        _context.Windows.Add(window);
        await _context.SaveChangesAsync();

        // Act
        var result = await _windowRepository.GetWindow(window.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(window.Id));
            Assert.That(result.Name, Is.EqualTo(window.Name));
            Assert.That(result.OrderId, Is.EqualTo(window.OrderId));
        });
    }

    [Test]
    public async Task AddWindow_WithValidWindow_AddsWindowToDatabase()
    {
        // Arrange
        var window = new Window { Name = "New Window" };

        // Act
        await _windowRepository.AddWindow(window);
        var result = await _windowRepository.GetWindows(new WindowPaginationQuery());

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.Items.Last().Name, Is.EqualTo("New Window"));
    }

    [Test]
    public async Task UpdateWindow_WithValidWindow_UpdatesWindowInDatabase()
    {
        // Arrange
        var newWindow = new Window { Name = "New Window" };

        // Act
        await _windowRepository.AddWindow(newWindow);
        var window = await _windowRepository.GetWindow(1);
        window.Name = "Updated Window";

        // Act
        await _windowRepository.UpdateWindow(window);
        var result = await _windowRepository.GetWindow(1);

        // Assert
        Assert.That(result.Name, Is.EqualTo("Updated Window"));
    }

    [Test]
    public async Task DeleteWindow_WithValidId_DeletesWindowFromDatabase()
    {
        // Arrange
        var window = new Window { Name = "New Window" };

        // Act
        await _windowRepository.AddWindow(window);
        var id = window.Id;
        await _windowRepository.DeleteWindow(id);
        var result = await _windowRepository.GetWindows(new WindowPaginationQuery());

        // Assert
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(result.Items.Any(w => w.Id == id), Is.False);
    }

    [Test]
    public async Task UpdateTotalSubElementCount_WithValidWindowId_UpdatesTotalSubElementCountInWindow()
    {
        // Arrange
        var order = new Order { Id = 999, Name = "New Order", State = "Test State" };
        var window = new Window { Id = 999, Name = "New Window" };
        var subElements = new List<SubElement>
        {
            new() { Type = "Doors", WindowId = window.Id, OrderId = order.Id, Element = 1, Width = 1100, Height = 500 },
            new() { Type = "Doors", WindowId = window.Id, OrderId = order.Id, Element = 1, Width = 1100, Height = 500 },
            new() { Type = "Doors", WindowId = window.Id, OrderId = order.Id, Element = 1, Width = 1100, Height = 500 },
            new() { Type = "Doors", WindowId = window.Id, OrderId = order.Id, Element = 1, Width = 1100, Height = 500 },
            new() { Type = "Doors", WindowId = window.Id, OrderId = order.Id, Element = 1, Width = 1100, Height = 500 }
        };

        // Act
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        await _windowRepository.AddWindow(window);
        _context.SubElements.AddRange(subElements);
        await _context.SaveChangesAsync();

        await _windowRepository.UpdateTotalSubElementCount(window.Id);
        var result = await _windowRepository.GetWindow(window.Id);

        // Assert
        Assert.That(result.TotalSubElements, Is.EqualTo(5));
    }
}