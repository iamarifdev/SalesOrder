namespace SalesOrder.Common.DTO.Window;

public class WindowUpdateDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Name { get; set; }
    public int QuantityOfWindows { get; set; }
}