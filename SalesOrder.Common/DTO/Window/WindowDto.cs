using SalesOrder.Common.DTO.Element;

namespace SalesOrder.Common.DTO.Window;

public class WindowDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Name { get; set; }
    public int QuantityOfWindows { get; set; }
    public int TotalSubElements { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public IEnumerable<SubElementDto> SubElements { get; set; } = new List<SubElementDto>();
}