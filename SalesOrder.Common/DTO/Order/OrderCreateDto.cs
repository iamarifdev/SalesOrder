using SalesOrder.Common.DTO.Window;

namespace SalesOrder.Common.DTO.Order;

public class OrderCreateDto
{
    public string Name { get; set; }

    public string State { get; set; }

    public List<WindowCreateDto> Windows { get; set; } = new();
}