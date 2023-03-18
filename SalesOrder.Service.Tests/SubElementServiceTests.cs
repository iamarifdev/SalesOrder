using AutoMapper;
using FluentAssertions;
using Moq;
using SalesOrder.Common.DTO.Element;
using SalesOrder.Common.Exceptions;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Service.Tests;

[TestFixture]
public class SubElementServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>();
        _subElementRepositoryMock = new Mock<ISubElementRepository>();
        _subElementService = new SubElementService(_mapperMock.Object, _subElementRepositoryMock.Object);
    }

    private ISubElementService _subElementService;
    private Mock<IMapper> _mapperMock;
    private Mock<ISubElementRepository> _subElementRepositoryMock;

    [Test]
    public async Task GetSubElements_ShouldReturnPaginatedList()
    {
        // Arrange
        var queryParams = new SubElementPaginationQuery { Page = 1, PageSize = 10 };
        var subElements = new List<SubElement>
        {
            new() { Id = 1, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 }
        };
        var paginatedSubElements = new PaginatedList<SubElement> { Items = subElements, Count = 1 };
        _subElementRepositoryMock.Setup(x => x.GetSubElements(queryParams)).ReturnsAsync(paginatedSubElements);
        _mapperMock.Setup(x => x.Map<PaginatedList<SubElementDto>>(paginatedSubElements)).Returns(
            new PaginatedList<SubElementDto>
            {
                Items = subElements.Select(w => new SubElementDto
                    {
                        Id = 1, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1
                    })
                    .ToList(),
                Count = 1
            });

        // Act
        var result = await _subElementService.GetSubElements(queryParams);

        // Assert
        result.Should().NotBeNull();
        result.Items.Count().Should().Be(1);
        result.Items.First().Id.Should().Be(1);
        result.Items.First().OrderId.Should().Be(1);
        result.Items.First().WindowId.Should().Be(1);
        result.Items.First().Type.Should().Be("Doors");
    }

    [Test]
    public async Task GetSubElement_ExistingSubElement_ShouldReturnSubElementDto()
    {
        // Arrange
        const int id = 1;
        var subElement = new SubElement
            { Id = 1, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };
        var subElementDto = new SubElementDto
            { Id = 1, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };

        _subElementRepositoryMock.Setup(x => x.GetSubElement(id)).ReturnsAsync(subElement);
        _mapperMock.Setup(x => x.Map<SubElementDto>(subElement)).Returns(subElementDto);

        // Act
        var result = await _subElementService.GetSubElement(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Type.Should().Be(subElement.Type);
        result.Height.Should().Be(subElement.Height);
        result.Width.Should().Be(subElement.Width);
    }

    [Test]
    public void GetSubElement_NonExistingSubElement_ShouldThrowSubElementNotFoundException()
    {
        // Arrange
        const int id = 1;

        _subElementRepositoryMock.Setup(x => x.GetSubElement(id)).ReturnsAsync((SubElement)null);

        // Act
        Func<Task> act = async () => await _subElementService.GetSubElement(id);

        // Assert
        act.Should().ThrowAsync<SubElementNotFoundException>();
    }

    [Test]
    public async Task AddSubElement_ValidSubElementCreateDto_ShouldReturnSubElementDto()
    {
        // Arrange
        var subElementCreateDto = new SubElementCreateDto
            { OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };
        var subElement = new SubElement
            { Id = 1, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };
        var subElementDto = new SubElementDto
            { Id = 1, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };

        _mapperMock.Setup(x => x.Map<SubElement>(subElementCreateDto)).Returns(subElement);
        _subElementRepositoryMock.Setup(x => x.AddSubElement(subElement)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<SubElementDto>(subElement)).Returns(subElementDto);

        // Act
        var result = await _subElementService.AddSubElement(subElementCreateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(subElement.Id);
        result.Type.Should().Be(subElement.Type);
        result.Height.Should().Be(subElement.Height);
        result.Width.Should().Be(subElement.Width);
    }

    [Test]
    public void UpdateSubElement_MismatchedId_ShouldThrowInvalidSubElementIdException()
    {
        // Arrange
        const int id = 1;
        var subElementUpdateDto = new SubElementUpdateDto
            { Id = 2, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };

        // Act
        Func<Task> act = async () => await _subElementService.UpdateSubElement(id, subElementUpdateDto);

        // Assert
        act.Should().ThrowAsync<InvalidSubElementIdException>();
    }

    [Test]
    public async Task UpdateSubElement_ValidSubElementUpdateDto_ShouldReturnUpdatedSubElementDto()
    {
        // Arrange
        const int id = 1;
        var subElementUpdateDto = new SubElementUpdateDto
            { Id = id, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1400, Width = 700, Element = 2 };
        var subElement = new SubElement
            { Id = id, OrderId = 1, WindowId = 1, Type = "Window", Height = 1200, Width = 600, Element = 1 };

        _subElementRepositoryMock.Setup(x => x.UpdateSubElement(It.IsAny<SubElement>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<SubElement>(subElementUpdateDto)).Returns(subElement);
        _mapperMock.Setup(x => x.Map<SubElementDto>(subElement)).Returns(new SubElementDto
            { Id = id, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1400, Width = 700, Element = 2 });

        // Act
        var result = await _subElementService.UpdateSubElement(id, subElementUpdateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Type.Should().Be(subElementUpdateDto.Type);
        result.Height.Should().Be(subElementUpdateDto.Height);
        result.Width.Should().Be(subElementUpdateDto.Width);
        result.Element.Should().Be(subElementUpdateDto.Element);
    }

    [Test]
    public async Task DeleteSubElement_ExistingSubElement_ShouldReturnDeletedSubElementDto()
    {
        // Arrange
        const int id = 1;
        var subElement = new SubElement
            { Id = id, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };
        var subElementDto = new SubElementDto
            { Id = id, OrderId = 1, WindowId = 1, Type = "Doors", Height = 1200, Width = 600, Element = 1 };

        _subElementRepositoryMock.Setup(x => x.GetSubElement(id)).ReturnsAsync(subElement);
        _subElementRepositoryMock.Setup(x => x.DeleteSubElement(id)).Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<SubElementDto>(subElement)).Returns(subElementDto);

        // Act
        var result = await _subElementService.DeleteSubElement(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Type.Should().Be(subElementDto.Type);
        result.Height.Should().Be(subElementDto.Height);
        result.Width.Should().Be(subElementDto.Width);
        result.Element.Should().Be(subElementDto.Element);
    }

    [Test]
    public void DeleteSubElement_NonExistingSubElement_ShouldThrowSubElementNotFoundException()
    {
        // Arrange
        const int id = 1;

        _subElementRepositoryMock.Setup(x => x.GetSubElement(id)).ReturnsAsync((SubElement)null);

        // Act
        Func<Task> act = async () => await _subElementService.DeleteSubElement(id);

        // Assert
        act.Should().ThrowAsync<SubElementNotFoundException>();
    }
}