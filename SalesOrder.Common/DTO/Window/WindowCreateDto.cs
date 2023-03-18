using SalesOrder.Common.DTO.Element;

namespace SalesOrder.Common.DTO.Window;

public class WindowCreateDto
{
    public int OrderId { get; set; }

    public string Name { get; set; }

    public int QuantityOfWindows { get; set; }

    public List<SubElementCreateDto> SubElements { get; set; } = new();
}