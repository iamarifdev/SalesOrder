using Microsoft.EntityFrameworkCore;
using SalesOrder.Common.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Data.Tests.Repositories;

[TestFixture]
public class SubElementRepositoryTests
{
    [SetUp]
    public async Task SetUpAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();

        _windowRepository = new WindowRepository(_dbContext);
        _subElementRepository = new SubElementRepository(_dbContext, _windowRepository);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    private ApplicationDbContext _dbContext;
    private ISubElementRepository _subElementRepository;
    private IWindowRepository _windowRepository;

    [Test]
    public async Task GetSubElements_ReturnsAllSubElements_WhenNoFiltersAreApplied()
    {
        // Arrange
        var subElements = TestDataHelper.GetTestSubElements();
        await _dbContext.SubElements.AddRangeAsync(subElements);
        await _dbContext.SaveChangesAsync();

        var expectedCount = subElements.Count;

        var queryParams = new SubElementPaginationQuery
        {
            Page = 1,
            PageSize = 10,
            SortField = "Id",
            SortOrder = "asc"
        };

        // Act
        var result = await _subElementRepository.GetSubElements(queryParams);

        // Assert
        Assert.That(result, Has.Count.EqualTo(expectedCount));
        Assert.That(result.Items, Is.EqualTo(subElements));
    }

    [Test]
    public async Task GetSubElements_ReturnsFilteredSubElements_WhenFiltersAreApplied()
    {
        // Arrange
        var subElements = TestDataHelper.GetTestSubElements();
        await _dbContext.SubElements.AddRangeAsync(subElements);
        await _dbContext.SaveChangesAsync();

        var expectedCount = subElements.Count(o => o.WindowId == 1 && o.OrderId == 123);

        var queryParams = new SubElementPaginationQuery
        {
            Page = 1,
            PageSize = 10,
            SortField = "Id",
            SortOrder = "asc",
            WindowId = 1,
            OrderId = 123
        };

        // Act
        var result = await _subElementRepository.GetSubElements(queryParams);

        // Assert
        Assert.That(result, Has.Count.EqualTo(expectedCount));
        Assert.That(result.Items, Is.EqualTo(subElements.Where(o => o.WindowId == 1 && o.OrderId == 123)));
    }

    [Test]
    public async Task GetSubElements_ReturnsPaginatedSubElements_WhenPaginationIsApplied()
    {
        // Arrange
        var subElements = TestDataHelper.GetTestSubElements();
        await _dbContext.SubElements.AddRangeAsync(subElements);
        await _dbContext.SaveChangesAsync();

        var expectedCount = 2;
        var expectedItems = subElements.Skip(2).Take(2).ToList();

        var queryParams = new SubElementPaginationQuery
        {
            Page = 2,
            PageSize = 2,
            SortField = "Id",
            SortOrder = "asc"
        };

        // Act
        var result = await _subElementRepository.GetSubElements(queryParams);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Items.Count(), Is.EqualTo(expectedCount));
            Assert.That(result.Items, Is.EqualTo(expectedItems));
        });
    }
}